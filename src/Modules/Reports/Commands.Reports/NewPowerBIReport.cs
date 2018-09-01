using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Client;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
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

        [Alias("ReportName")]
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = false)]
        public Workspace Workspace { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet(
            "Abort",
            "CreateOrOverwrite",
            "Ignore",
            "Overwrite"
        )]
        [PSDefaultValue(Value = "CreateOrOverwrite")]
        public string ConflictAction { get; set; }
        #endregion

        #region Constructors
        public NewPowerBIReport() : base() { }

        public NewPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            if (this.Name == null)
            {
                var report = new System.IO.FileInfo(this.Path);
                this.Name = report.Name.Replace(".pbix", "");
            }

            ImportConflictHandlerModeEnum conflictAction = ImportConflictHandlerModeEnum.CreateOrOverwrite;
            if (this.ConflictAction != null && !Enum.TryParse(this.ConflictAction, out conflictAction))
            {
                this.Logger.WriteError("Failed to parse ConflictAction");
            }

            using (var client = this.CreateClient())
            {
                Report report;
                if (this.WorkspaceId != default)
                {
                    report = client.Reports.PostReportForWorkspace(this.WorkspaceId, this.Name, this.Path, conflictAction);
                }
                else
                {
                    report = client.Reports.PostReport(this.Name, this.Path, conflictAction);
                }
                this.Logger.WriteObject(report, false);
            }
        }
    }
}
