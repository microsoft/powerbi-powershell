/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class RemovePowerBIWorkspaceUser : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspaceUser";
        public const string CmdletVerb = VerbsCommon.Remove;

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("UserEmailAddress")]
        public string UserPrincipalName { get; set; }

        #endregion

        protected override void ExecuteCmdlet()
        {
            IPowerBIClient client = this.CreateClient();

            var result = this.Scope.Equals(PowerBIUserScope.Individual) ? client.Groups.DeleteUserInGroup(this.Id.ToString(), this.UserPrincipalName) : client.Groups.DeleteUserAsAdmin(this.Id.ToString(), this.UserPrincipalName);
            this.Logger.WriteObject(result, true);
        }
    }
}
