/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = IdParameterSetName)]
    [Alias("Restore-PowerBIGroup")]
    public class RestorePowerBIWorkspace : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsData.Restore;

        private const string IdParameterSetName = "Id";
        private const string WorkspaceParameterSetName = "Workspace";

        public RestorePowerBIWorkspace() : base() { }

        public RestorePowerBIWorkspace(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = false, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = WorkspaceParameterSetName)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Organization;

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName, ValueFromPipelineByPropertyName = true)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        public string RestoredName { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("AdminEmailAddress")]
        public string AdminUserPrincipalName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName)]
        [Alias("Group")]
        public Workspace Workspace { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.Scope.Equals(PowerBIUserScope.Individual))
            {
                throw new NotImplementedException($"{CmdletVerb}-{CmdletName} is only supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }

            if (this.Scope == PowerBIUserScope.Organization)
            {
                this.Logger.WriteWarning($"Only workspaces created in the new workspace experience are supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }
        }

        public override void ExecuteCmdlet()
        {
            var workspaceId = this.ParameterSet.Equals(IdParameterSetName) ? this.Id : this.Workspace.Id;
            var restoreRequest = new WorkspaceRestoreRequest { RestoredName = this.RestoredName, AdminUserPrincipalName = this.AdminUserPrincipalName };

            using (var client = this.CreateClient())
            {
                client.Workspaces.RestoreDeletedWorkspaceAsAdmin(workspaceId, restoreRequest);
            }
        }
    }
}
