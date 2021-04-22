/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = MyWorkspaceParameterSetName)]
    public class RemovePowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIReport";
        public const string CmdletVerb = VerbsCommon.Remove;

        #region ParameterSets

        private const string MyWorkspaceParameterSetName = "MyWorkspace";
        private const string WorkspaceIdParameterSetName = "WorkspaceId";
        private const string WorkspaceParameterSetName = "Workspace";

        #endregion

        #region ParameterSets

        [Parameter(Mandatory = true)]
        [Alias("ReportId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceIdParameterSetName)]
        [Alias("GroupId")]
        public Guid WorkspaceId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName)]
        [Alias("Group")]
        public Workspace Workspace { get; set; }

        #endregion

        #region Constructors
        public RemovePowerBIReport() : base() { }

        public RemovePowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (this.Workspace != default)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            using (var client = this.CreateClient())
            {
                object result = null;
                if (this.WorkspaceId != default)
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
