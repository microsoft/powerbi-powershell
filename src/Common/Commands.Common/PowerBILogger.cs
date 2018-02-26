using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Management.Automation;
using System.Threading;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    public class PowerBILogger : IPowerBILogger
    {
        private ConcurrentQueue<object> debugQueue = new ConcurrentQueue<object>();
        private ConcurrentQueue<ErrorRecord> errorQueue = new ConcurrentQueue<ErrorRecord>();
        private ConcurrentQueue<Tuple<object, ConsoleColor?, ConsoleColor?>> hostQueue = new ConcurrentQueue<Tuple<object, ConsoleColor?, ConsoleColor?>>();
        private ConcurrentQueue<Tuple<object, bool?>> outputQueue = new ConcurrentQueue<Tuple<object, bool?>>();
        private ConcurrentQueue<object> verboseQueue = new ConcurrentQueue<object>();
        private ConcurrentQueue<object> warningQueue = new ConcurrentQueue<object>();

        public PSCmdlet Cmdlet { get; set; }

        protected bool IgnoreCommandRuntime { get; set; }

        protected bool IgnoreQueueForThreads { get; set; }

        protected Action<object> DebugListener { get; set; }

        protected Action<ErrorRecord> ErrorListener { get; set; }

        protected Action<object> HostListener { get; set; }

        protected Action<object> VerboseListener { get; set; }

        protected Action<object> WarningListener { get; set; }

        protected Action<object> OutputListener { get; set; }

        private IPowerBICmdlet pbiCmdlet;
        private bool testedCmdlet;
        protected IPowerBICmdlet PowerBICmdlet
        {
            get
            {
               if(testedCmdlet)
               {
                    return pbiCmdlet;
               }

                testedCmdlet = true;
                pbiCmdlet = this.Cmdlet as IPowerBICmdlet;
                return pbiCmdlet;
            }
        }

        protected bool IsMainThread
        {
            get
            {
                if(this.PowerBICmdlet != null)
                {
                    return this.PowerBICmdlet.MainThreadId == Thread.CurrentThread.ManagedThreadId;
                }

                return true;
            }
        }

        private bool CommandRuntimeAvailable => this.IgnoreCommandRuntime || this.Cmdlet?.CommandRuntime != null;

        private bool OnDifferentThread => !this.IgnoreQueueForThreads && !this.IsMainThread;

        public void FlushMessages()
        {
            if(!this.IsMainThread)
            {
                Trace.TraceWarning("Called FlushMessages() on non-main thread, ignored");
                return;
            }

            while(this.debugQueue.TryDequeue(out object debugMessage))
            {
                this.WriteDebug(debugMessage);
            }

            while(this.verboseQueue.TryDequeue(out object verboseMessage))
            {
                this.WriteVerbose(verboseMessage);
            }

            while(this.hostQueue.TryDequeue(out Tuple<object, ConsoleColor?, ConsoleColor?> hostMessage))
            {
                this.WriteHost(hostMessage.Item1, hostMessage.Item2, hostMessage.Item3);
            }

            while(this.outputQueue.TryDequeue(out Tuple<object, bool?> outputMessage))
            {
                this.WriteObject(outputMessage.Item1, outputMessage.Item2);
            }

            while(this.warningQueue.TryDequeue(out object warningMessage))
            {
                this.WriteWarning(warningMessage);
            }

            while (this.errorQueue.TryDequeue(out ErrorRecord errorMessage))
            {
                this.WriteError(errorMessage);
            }
        }

        public void WriteDebug(object obj)
        {
            if(this.OnDifferentThread)
            {
                this.debugQueue.Enqueue(obj);
            }
            else if(this.CommandRuntimeAvailable)
            {
                this.Cmdlet.WriteDebug(obj.ToString());
                this.DebugListener?.Invoke(obj);
            }

            Trace.Write("[DEBUG]:: ");
            Trace.WriteLine(obj);
        }

        public void WriteError(object obj, ErrorCategory category = ErrorCategory.WriteError)
        {
            var errorRecord = new ErrorRecord(new Exception(obj.ToString()), obj.ToString(), category, this.Cmdlet);
            this.WriteError(errorRecord);
        }

        public void WriteError(Exception ex, ErrorCategory category = ErrorCategory.WriteError)
        {
            var errorRecord = new ErrorRecord(ex, ex.Message, category, this.Cmdlet);
            this.WriteError(errorRecord);
        }

        public void WriteError(object obj, Exception ex, ErrorCategory category = ErrorCategory.WriteError)
        {
            var errorRecord = new ErrorRecord(ex, obj.ToString(), category, this.Cmdlet);
            this.WriteError(errorRecord);
        }

        public void WriteError(ErrorRecord record)
        {
            if (this.OnDifferentThread)
            {
                this.errorQueue.Enqueue(record);
            }
            else if (this.CommandRuntimeAvailable)
            {
                this.Cmdlet.WriteError(record);
                this.ErrorListener?.Invoke(record);
            }

            Trace.TraceError(record.ToString());
        }

        public void WriteHost(object obj, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (this.OnDifferentThread)
            {
                var hostMessage = new Tuple<object, ConsoleColor?, ConsoleColor?>(obj, foregroundColor, backgroundColor);
                this.hostQueue.Enqueue(hostMessage);
            }
            else if (this.CommandRuntimeAvailable && this.Cmdlet.Host?.UI != null)
            {
                if (foregroundColor == null && backgroundColor == null)
                {
                    this.Cmdlet.Host.UI.WriteLine(obj.ToString());
                }
                else
                {
                    if(foregroundColor == null)
                    {
                        foregroundColor = this.Cmdlet.Host.UI.RawUI.ForegroundColor;
                    }

                    if(backgroundColor == null)
                    {
                        backgroundColor = this.Cmdlet.Host.UI.RawUI.BackgroundColor;
                    }

                    this.Cmdlet.Host.UI.WriteLine(foregroundColor.Value, backgroundColor.Value, obj.ToString());
                    this.HostListener?.Invoke(obj);
                }
            }

            Trace.Write("[HOST]:: ");
            Trace.WriteLine(obj);
        }

        public void WriteObject(object obj, bool? enumerateCollection = null)
        {
            if (this.OnDifferentThread)
            {
                var outputMessage = new Tuple<object, bool?>(obj, enumerateCollection);
                this.outputQueue.Enqueue(outputMessage);
            }
            else if (this.CommandRuntimeAvailable)
            {
                this.Cmdlet.WriteObject(obj, enumerateCollection.GetValueOrDefault());
                this.OutputListener?.Invoke(obj);
            }

            Trace.Write("[OUT]:: ");
            Trace.WriteLine(obj);
        }

        public void WriteVerbose(object obj)
        {
            if (this.OnDifferentThread)
            {
                this.verboseQueue.Enqueue(obj);
            }
            else if (this.CommandRuntimeAvailable)
            {
                this.Cmdlet.WriteVerbose(obj.ToString());
                this.VerboseListener?.Invoke(obj);
            }

            Trace.Write("[VERBOSE]:: ");
            Trace.WriteLine(obj);
        }

        public void WriteWarning(object obj)
        {
            if (this.OnDifferentThread)
            {
                this.warningQueue.Enqueue(obj);
            }
            else if (this.CommandRuntimeAvailable)
            {
                this.Cmdlet.WriteWarning(obj.ToString());
                this.WarningListener?.Invoke(obj);
            }

            Trace.TraceWarning(obj.ToString());
        }
    }
}