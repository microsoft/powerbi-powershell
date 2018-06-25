using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Microsoft.PowerBI.Commands.Profile.Errors
{
    public class PowerBIExceptionRecord : PowerBIErrorRecord
    {
        public Exception Exception { get; }
        public bool InnerException { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string HelpLink { get; set; }
        public string Source { get; set; }

        public PowerBIExceptionRecord(Exception exception, ErrorRecord record, bool inner = false) : base(record)
        {
            if(exception != null)
            {
                this.Message = exception.Message;
                this.HelpLink = exception.HelpLink;
                this.StackTrace = exception.StackTrace;
                this.Source = exception.Source;
            }

            this.Exception = exception;
        }
    }
}
