using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(void))]

    public class RemovePowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIReport";
        public const string CmdletVerb = VerbsCommon.Remove;

        [Parameter(Mandatory = true)]
        [Alias("ReportId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("GroupId")]
        public Guid WorkspaceId { get; set; }

        public RemovePowerBIReport() : base() { }

        public RemovePowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }


        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }


        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                object result = null;
                if (WorkspaceId != default)
                {
                    result = client.Reports.DeleteReport(this.WorkspaceId, this.Id);
                }
                else
                {
                    result = client.Reports.DeleteReport(this.Id);
                }
                
                this.Logger.WriteObject(result, true);
            }
        }
    }
}
