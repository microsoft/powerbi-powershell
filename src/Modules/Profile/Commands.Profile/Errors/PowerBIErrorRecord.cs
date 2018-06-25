using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Microsoft.PowerBI.Commands.Profile.Errors
{
    public class PowerBIErrorRecord
    {
        public ErrorDetails ErrorDetails { get; set; }
        public ErrorCategoryInfo ErrorCategory { get; set; }
        public InvocationInfo InvocationInfo { get; set; }
        public string ScriptStackTrace { get; set; }

        public PowerBIErrorRecord(ErrorRecord record)
        {
            if(record == null)
            {
                return;
            }

            this.InvocationInfo = record.InvocationInfo;
            this.ErrorDetails = record.ErrorDetails;
            this.ScriptStackTrace = record.ScriptStackTrace;
            this.ErrorCategory = record.CategoryInfo;
        }
    }
}
