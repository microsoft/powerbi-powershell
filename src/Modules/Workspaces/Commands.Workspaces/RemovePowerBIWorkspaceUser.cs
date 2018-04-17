﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = IdParameterSetName)]
    [Alias("Remove-PowerBIGroupUser")]
    public class RemovePowerBIWorkspaceUser : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspaceUser";
        public const string CmdletVerb = VerbsCommon.Remove;

        private const string IdParameterSetName = "Id";
        private const string WorkspaceParameterSetName = "Workspace";

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName, ValueFromPipelineByPropertyName = true)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("UserEmailAddress")]
        public string UserPrincipalName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName)]
        [Alias("Group")]
        public Group Workspace { get; set; }

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
            var client = this.CreateClient();

            var workspaceId = this.ParameterSetName.Equals(IdParameterSetName) ? this.Id.ToString() : this.Workspace.Id.ToString();
            var result = this.Scope.Equals(PowerBIUserScope.Individual) ? 
                client.Groups.DeleteUserInGroup(workspaceId, this.UserPrincipalName) : 
                client.Groups.DeleteUserAsAdmin(workspaceId, this.UserPrincipalName);
            this.Logger.WriteObject(result, true);
        }
    }
}
