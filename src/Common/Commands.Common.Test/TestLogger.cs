using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using System.Linq;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestLogger : IPowerBILogger
    {
        private ConcurrentQueue<object> debugQueue = new ConcurrentQueue<object>();
        private ConcurrentQueue<ErrorRecord> errorQueue = new ConcurrentQueue<ErrorRecord>();
        private ConcurrentQueue<object> hostQueue = new ConcurrentQueue<object>();
        private ConcurrentQueue<object> outputQueue = new ConcurrentQueue<object>();
        private ConcurrentQueue<object> verboseQueue = new ConcurrentQueue<object>();
        private ConcurrentQueue<object> warningQueue = new ConcurrentQueue<object>();

        public int FlushMessageCount { get; private set; }

        public PSCmdlet Cmdlet { get; set; }

        public void FlushMessages()
        {
            this.FlushMessageCount++;
        }

        public IEnumerable<string> DebugMessages => this.debugQueue.Select(d => d.ToString());

        public IEnumerable<ErrorRecord> ErrorRecords => this.errorQueue.Select(e => e);

        public IEnumerable<string> HostMessages => this.hostQueue.Select(h => h.ToString());

        public IEnumerable<object> Output => this.outputQueue.Select(o => o);

        public IEnumerable<string> VerboseMessages => this.verboseQueue.Select(v => v.ToString());

        public IEnumerable<string> WarningMessages => this.warningQueue.Select(w => w.ToString());

        public void WriteDebug(object obj)
        {
            this.debugQueue.Enqueue(obj);
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
            this.errorQueue.Enqueue(record);
        }

        public void WriteHost(object obj, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            this.hostQueue.Enqueue(obj);
        }

        public void WriteObject(object obj, bool? enumerateCollection = null)
        {
            this.outputQueue.Enqueue(obj);
        }

        public void WriteVerbose(object obj)
        {
            this.verboseQueue.Enqueue(obj);
        }

        public void WriteWarning(object obj)
        {
            this.warningQueue.Enqueue(obj);
        }
    }
}
