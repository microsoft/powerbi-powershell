using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Client;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Abstractions;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(Report))]
    public class NewPowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.New;
        public const string CmdletName = "PowerBIReport";

        #region Parameters

        [Parameter(Position = 0, Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }
        #endregion

        #region Constructors
        public NewPowerBIReport() : base() { }

        public NewPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Organization && this.WorkspaceId != default)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.WorkspaceId)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Individual)}", ErrorCategory.InvalidArgument);
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.Name == null)
            {
                var report = new System.IO.FileInfo(this.Path);
                this.Name = report.Name.Replace(".pbix", "");
            }

            using (var client = this.CreateClient())
            {
                Report report;
                if (this.WorkspaceId != default)
                {
                    report = client.Reports.PostReportForWorkspace(this.WorkspaceId, this.Name, this.Path);
                }
                else
                {
                    report = client.Reports.PostReport(this.Name, this.Path);
                }
                this.Logger.WriteObject(report, false);
            }
        }
    }
}
