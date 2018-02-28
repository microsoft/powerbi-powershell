/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Authentication;

namespace Microsoft.PowerBI.Commands.Common
{
    public abstract class PowerBICmdlet : PSCmdlet, IPowerBICmdlet
    {
        #region DI Properties
        protected IPowerBILoggerFactory LoggerFactory { get; }
        protected IDataStorage Storage { get; }
        protected IAuthenticationFactory Authenticator { get; }
        protected IPowerBISettings Settings { get; }
        #endregion

        private static IServiceProvider Provider { get; set; }

        protected static readonly string SessionId = Guid.NewGuid().ToString();

        private bool? interactive;
        private object lockObject = new object();

        public PowerBICmdlet() : this(GetInstance<IPowerBICmdletInitFactory>()) { }

        public PowerBICmdlet(IPowerBICmdletInitFactory init)
        {
            (this.LoggerFactory, this.Storage, this.Authenticator, this.Settings) = init;
            this.MainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        static PowerBICmdlet()
        {
            AppDomain.CurrentDomain.AssemblyResolve += RedirectAssemblyLoad;

            var serviceCollection = new ServiceCollection()
                .AddSingleton<IPowerBILoggerFactory, PowerBILoggerFactory>()
                .AddSingleton<IDataStorage, ModuleDataStorage>()
                .AddSingleton<IPowerBISettings, PowerBISettings>()
                .AddSingleton<IPowerBICmdletInitFactory, PowerBICmdletInitFactory>()
                .AddSingleton<IAuthenticationFactory, AuthenticationFactorySelector>();

            SetProvider(serviceCollection);
        }

        protected static void SetProvider(IServiceCollection serviceCollection)
        {
            Provider = serviceCollection.BuildServiceProvider();
        }

        private static Assembly RedirectAssemblyLoad(object sender, ResolveEventArgs args)
        {
            /*
             * Using an assembly resolver mainly to load System.Net.Http due to a bug introduced in .NET Framework and later fixed but causes assembly version mismatch.
             * Assembly resolver was the clear choice as the System.Net.Http package only fixed the issue in PowerShell Core but would fail when running from PowerShell for Windows.
             */
            var requestedAssembly = new AssemblyName(args.Name);
            string executingDirectory = GetExecutingDirectory();
            string assemblyFilePath = Path.Combine(executingDirectory, requestedAssembly.Name + ".dll");
            if (File.Exists(assemblyFilePath))
            {
                return Assembly.LoadFrom(assemblyFilePath);
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName.Equals(args.Name))
                {
                    return asm;
                }
            }

            return null;
        }

        protected static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var fileUri = new UriBuilder(codeBase);
            var directory = Uri.UnescapeDataString(fileUri.Path);
            directory = Path.GetDirectoryName(directory);
            return directory;
        }

        private IPowerBILogger logger;
        protected IPowerBILogger Logger
        {
            get
            {
                if (this.logger == null)
                {
                    lock (this.lockObject)
                    {
                        if (this.logger != null) { return this.logger; }

                        this.logger = this.LoggerFactory.CreateLogger(this);
                    }
                }

                return this.logger;
            }
        }

        protected static T GetInstance<T>() => Provider.GetService<T>();

        protected IPowerBIProfile Profile => this.Storage.TryGetItem<IPowerBIProfile>("profile", out IPowerBIProfile profile) ? profile : null;
        
        protected virtual bool CmdletManagesProfile { get; set; }

        protected void AssertProfileExists()
        {
            if(this.Profile == null)
            {
                throw new Exception("Login first with Login-PowerBIServiceAccount");
            }
        }

        public int MainThreadId { get; }

        protected bool InteractiveConsole
        {
            get
            {
                if(this.interactive.HasValue)
                {
                    return this.interactive.Value;
                }

                this.interactive = true;
                if (this.Host == null || 
                    this.Host.UI == null || 
                    this.Host.UI.RawUI == null || 
                    Environment.GetCommandLineArgs().Any(s => s.Equals("-NonInteractive", StringComparison.OrdinalIgnoreCase)))
                {
                    this.interactive = false;
                }
                else
                {
                    try
                    {
                        var test = this.Host.UI.RawUI.KeyAvailable;
                    }
                    catch
                    {
                        this.interactive = false;
                    }
                }

                return interactive.Value;
            }
        }

        protected virtual bool IsTerminatingError(Exception ex) => ex is PipelineStoppedException pipelineStoppedEx && pipelineStoppedEx.InnerException == null;

        protected virtual string CurrentPath => this.SessionState != null ? SessionState.Path.CurrentLocation.Path : Directory.GetCurrentDirectory();

        protected virtual bool IsVerbose
        {
            get
            {
                if(this.MyInvocation == null)
                {
                    return false;
                }

                return this.MyInvocation.BoundParameters.ContainsKey("Verbose") && ((SwitchParameter)this.MyInvocation.BoundParameters["Verbose"]).ToBool();
            }
        }

        protected override void BeginProcessing()
        {
            LogCmdletStartInvocationInfo();
            if(!this.CmdletManagesProfile)
            {
                AssertProfileExists();
            }

            base.BeginProcessing();
        }

        protected override void EndProcessing()
        {
            this.Logger?.FlushMessages();
            this.LogCmdletEndInvocationInfo();
            base.EndProcessing();
        }

        protected abstract void ExecuteCmdlet();
        protected virtual string ModuleName => this.MyInvocation?.MyCommand.ModuleName ?? this.GetType().Module.Name;

        protected virtual string CommandName => this.MyInvocation?.MyCommand.Name ?? this.GetType().Name;

        protected override void ProcessRecord()
        {
            try
            {
                base.ProcessRecord();
                this.ExecuteCmdlet();
                this.Logger?.FlushMessages();
            }
            catch(Exception ex) when (!this.IsTerminatingError(ex))
            {
                if (this.Logger != null)
                {
                    this.Logger.WriteError(ex);
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual void LogCmdletStartInvocationInfo()
        {
            var message = string.IsNullOrEmpty(this.ParameterSetName) ? 
                $"{this.CommandName} begin processing without ParameterSet." : 
                $"{this.CommandName} begin processing with ParameterSet {this.ParameterSetName}.";
            this.WriteDebugWithTimestamp(message);
        }

        protected virtual void LogCmdletEndInvocationInfo() => this.WriteDebugWithTimestamp($"{this.CommandName} end processing.");

        protected void WriteDebugWithTimestamp(string message) => this.Logger?.WriteDebug(string.Format("{0:T} - {1}", DateTime.Now, message));
    }
}