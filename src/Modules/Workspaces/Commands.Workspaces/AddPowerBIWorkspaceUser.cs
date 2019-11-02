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
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = UserEmailWithIdParameterSetName)]
    [Alias("Add-PowerBIGroupUser")]
    public class AddPowerBIWorkspaceUser : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspaceUser";
        public const string CmdletVerb = VerbsCommon.Add;

        private const string UserEmailWithIdParameterSetName = "UserEmailWithId";
        private const string UserEmailWithWorkspaceParameterSetName = "UserEmailWithWorkspace";

        private const string PrincipalTypeWithIdParameterSetName = "PrincipalTypeWithId";
        private const string PrincipalTypeWithWorkspaceParameterSetName = "PrincipalTypeWithWorkspace";

        public AddPowerBIWorkspaceUser() : base() { }

        public AddPowerBIWorkspaceUser(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true, ParameterSetName = UserEmailWithIdParameterSetName, ValueFromPipelineByPropertyName = true)]
        [Parameter(Mandatory = true, ParameterSetName = PrincipalTypeWithIdParameterSetName, ValueFromPipelineByPropertyName = true)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserEmailWithIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = UserEmailWithWorkspaceParameterSetName)]
        [Alias("UserEmailAddress")]
        public string UserPrincipalName { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("UserAccessRight")]
        public WorkspaceUserAccessRight AccessRight { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = UserEmailWithWorkspaceParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = PrincipalTypeWithWorkspaceParameterSetName)]
        [Alias("Group")]
        public Workspace Workspace { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PrincipalTypeWithIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = PrincipalTypeWithWorkspaceParameterSetName)]
        public WorkspaceUserPrincipalType? PrincipalType { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PrincipalTypeWithIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = PrincipalTypeWithWorkspaceParameterSetName)]
        [Alias("PrincipalId")]
        public string Identifier { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.Scope == PowerBIUserScope.Organization)
            {
                this.Logger.WriteWarning($"Only preview workspaces are supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }
        }

        public override void ExecuteCmdlet()
        {
            var workspaceId = (this.ParameterSet.Equals(UserEmailWithIdParameterSetName) || this.ParameterSet.Equals(PrincipalTypeWithIdParameterSetName)) ? this.Id : this.Workspace.Id;
            bool usingPrincipalType = this.ParameterSet.Equals(PrincipalTypeWithIdParameterSetName) || this.ParameterSet.Equals(PrincipalTypeWithWorkspaceParameterSetName);

            WorkspaceUser userAccessRight;
            if (usingPrincipalType)
            {
                userAccessRight = new WorkspaceUser { AccessRight = this.AccessRight.ToString(), Identifier = this.Identifier, PrincipalType = this.PrincipalType };
            }
            else
            {
                userAccessRight = new WorkspaceUser { AccessRight = this.AccessRight.ToString(), UserPrincipalName = this.UserPrincipalName };
            }

            using (var client = this.CreateClient())
            {
                var result = this.Scope.Equals(PowerBIUserScope.Individual) ?
                    client.Workspaces.AddWorkspaceUser(workspaceId, userAccessRight) :
                    client.Workspaces.AddWorkspaceUserAsAdmin(workspaceId, userAccessRight);
                this.Logger.WriteObject(result, true);
            }
        }
    }
}