/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = WorkspaceParameterSetName)]
    [Alias("Get-PowerBIGroupMigrationStatus")]
    [OutputType(typeof(WorkspaceLastMigrationStatus))]
    public class GetPowerBIWorkspaceMigrationStatus : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIWorkspaceMigrationStatus";
        public const string CmdletVerb = VerbsCommon.Get;

        private const string IdParameterSetName = "Id";
        private const string WorkspaceParameterSetName = "Workspace";

        public GetPowerBIWorkspaceMigrationStatus() : base() { }

        public GetPowerBIWorkspaceMigrationStatus(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName)]
        [Alias("Group")]
        public Workspace Workspace { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            var workspaceId = this.ParameterSet.Equals(IdParameterSetName) ? this.Id : this.Workspace.Id;

            using (var client = this.CreateClient())
            {
                var status = client.Workspaces.GetWorkspaceLastMigrationStatus(workspaceId);
                this.Logger.WriteObject(status);
            }
        }
    }
}