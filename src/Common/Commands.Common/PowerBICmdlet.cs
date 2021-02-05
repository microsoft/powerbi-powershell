/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    public abstract class PowerBICmdlet : PSCmdlet, IPowerBICmdlet
    {
        #region Properties
        protected IPowerBILoggerFactory LoggerFactory { get; }
        protected IDataStorage Storage { get; }
        protected IAuthenticationFactory Authenticator { get; }
        protected IPowerBISettings Settings { get; }
        #endregion

        public static readonly string CmdletVersion = typeof(PowerBICmdlet).Assembly.GetName().Version.ToString();

        private bool? interactive;
        private object lockObject = new object();

        public PowerBICmdlet() : this(GetDefaultInitFactory()) { }

        public PowerBICmdlet(IPowerBICmdletInitFactory init)
        {
            (this.LoggerFactory, this.Storage, this.Authenticator, this.Settings) = init;
            this.MainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        static PowerBICmdlet()
        {
            AppDomain.CurrentDomain.AssemblyResolve += RedirectAssemblyLoad;
        }

        protected static readonly bool IsNetFramework = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework");

        private static Assembly RedirectAssemblyLoad(object sender, ResolveEventArgs args)
        {
            /*
            * Situations Assembly Resolver is used:
            * 
            * The assembly resolver is used to load System.Net.Http due to a bug introduced in .NET Framework and later fixed but causes assembly version mismatch.
            * Assembly resolver was the clear choice as the System.Net.Http package only fixed the issue in PowerShell Core but would fail when running from PowerShell for Windows.
            * 
            * It is used to load MSAL assemblies as the netstandard2.0 (or in MSAL case, netstandard1.3) will throw PlatformNotSupported when used.
            * Both net45 and netcoreapp2.1 are packaged up and placed in an MSAL directory under the module.
            * 
            * Finally it is used to fix mismatches with Newtonsoft.Json.
            * In this situation, it's a best effort because if a very old version of Newtonsoft.Json is loaded and it's contract is incompatible with used in codebase, then we will run into type mismatch errors which we can't handle.
            */
            var requestedAssembly = new AssemblyName(args.Name);
            string executingDirectory = GetExecutingDirectory();

            // Handle MSAL assemblies
            string assemblyFilePath;
            if (requestedAssembly.Name == "Microsoft.Identity.Client" || requestedAssembly.Name == "Microsoft.Identity.Client.Extensions.Msal")
            {
                var libType = IsNetFramework ? "net45" : "netcoreapp2.1";
                assemblyFilePath = Path.Combine(executingDirectory, "MSAL", libType, requestedAssembly.Name + ".dll");
            }
            else
            {
                assemblyFilePath = Path.Combine(executingDirectory, requestedAssembly.Name + ".dll");
            }

            if (File.Exists(assemblyFilePath))
            {
                try
                {
                    return Assembly.LoadFrom(assemblyFilePath);
                }
                catch (Exception)
                {
                    // Ignore as the assembly resolver is meant as a last resort to find an assembly
                }
            }

            // This allows usage of assemblies in the current application domain to be used when a specific version of an assembly wasn't found
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName.Equals(args.Name))
                {
                    return asm;
                }
            }

            // Return null to indicate the assembly was not found
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

        protected static IPowerBICmdletInitFactory GetDefaultInitFactory() => new PowerBICmdletInitFactory(new PowerBILoggerFactory(), new ModuleDataStorage(), new AuthenticationFactorySelector(), new PowerBISettings());

        protected IPowerBIProfile Profile => this.Storage.TryGetItem<IPowerBIProfile>("profile", out IPowerBIProfile profile) ? profile : null;
        
        protected virtual bool CmdletManagesProfile { get; set; }

        protected void AssertProfileExists()
        {
            if(this.Profile == null)
            {
                throw new Exception("Login first with Login-PowerBIServiceAccount");
            }
        }

        private string parameterSet;
        /// <summary>
        /// The name of the current parameter set.
        /// </summary>
        /// <remarks>
        /// ParameterSet should be used in place of ParameterSetName for cmdlets in order to enable unit testing.
        /// </remarks>
        public string ParameterSet
        {
            get => this.parameterSet ?? this.ParameterSetName;
            set => this.parameterSet = value;
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

        protected virtual bool IsTerminatingError(Exception ex) => ex is PipelineStoppedException pipelineStoppedEx && pipelineStoppedEx.InnerException == null || ex is NotImplementedException;

        protected virtual string CurrentPath => this.SessionState != null ? this.SessionState.Path.CurrentLocation.Path : Directory.GetCurrentDirectory();

        protected virtual string ResolveFilePath(string path, bool isLiteralPath)
        {
            ProviderInfo provider = null;
            PSDriveInfo drive = null;
            var filePaths = new List<string>();

            if (this.SessionState != null)
            {
                try
                {
                    if (isLiteralPath)
                    {
                        filePaths.Add(this.SessionState.Path.GetUnresolvedProviderPathFromPSPath(path, out provider, out drive));
                    }
                    else
                    {
                        filePaths.AddRange(this.SessionState.Path.GetResolvedProviderPathFromPSPath(path, out provider));
                    }

                    if (provider != null && !provider.Name.Contains("FileSystem"))
                    {
                        this.logger.ThrowTerminatingError(new NotSupportedException($"Unsupported PowerShell provider '{provider.Name}' for resolving path: {path}"));
                    }

                    if (filePaths.Count > 1)
                    {
                        this.logger.ThrowTerminatingError(new NotSupportedException("Multiple files not supported"), ErrorCategory.InvalidArgument);
                    }

                    if (filePaths.Count == 0)
                    {
                        this.logger.ThrowTerminatingError(new FileNotFoundException("Wildcard unable to find file"), ErrorCategory.OpenError);
                    }

                    return filePaths[0];
                }
                catch (ItemNotFoundException)
                {
                    path = this.SessionState.Path.GetUnresolvedProviderPathFromPSPath(path, out provider, out drive);
                    if (provider != null && !provider.Name.Contains("FileSystem"))
                    {
                        this.logger.ThrowTerminatingError(new NotSupportedException($"Unsupported PowerShell provider '{provider.Name}' for resolving path: {path}"));
                    }

                    return path;
                }
            }
            else
            {
                if (Path.IsPathRooted(path))
                {
                    return path;
                }

                return Path.Combine(this.CurrentPath, path);
            }
        }

        protected virtual bool IsVerbose
        {
            get
            {
                if (this.MyInvocation == null)
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

        public abstract void ExecuteCmdlet();
        protected virtual string ModuleName => this.MyInvocation?.MyCommand?.ModuleName ?? this.GetType().Module.Name;

        protected virtual string CommandName => this.MyInvocation?.MyCommand?.Name ?? this.GetType().Name;

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
            var message = string.IsNullOrEmpty(this.ParameterSet) ? 
                $"{this.CommandName} begin processing without ParameterSet." : 
                $"{this.CommandName} begin processing with ParameterSet {this.ParameterSet}.";
            this.WriteDebugWithTimestamp(message);
            this.WriteDebugWithTimestamp($"Cmdlet version: {CmdletVersion}");
        }

        protected virtual void LogCmdletEndInvocationInfo() => this.WriteDebugWithTimestamp($"{this.CommandName} end processing.");

        protected void WriteDebugWithTimestamp(string message) => this.Logger?.WriteDebug(string.Format("{0:T} - {1}", DateTime.Now, message));
    }
}