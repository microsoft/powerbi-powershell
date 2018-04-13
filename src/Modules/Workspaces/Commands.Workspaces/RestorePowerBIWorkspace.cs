﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = PropertiesParameterSetName)]
    [Alias("Restore-PowerBIGroup")]
    public class RestorePowerBIWorkspace : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsData.Restore;

        private const string PropertiesParameterSetName = "Properties";
        private const string WorkspaceParameterSetName = "Workspace";

        #region Parameters

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = WorkspaceParameterSetName)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true, ParameterSetName = PropertiesParameterSetName, ValueFromPipelineByPropertyName = true)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("UserEmailAddress")]
        public string UserPrincipalName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName)]
        [Alias("Group")]
        public Group Workspace { get; set; }

        #endregion

        protected override void ExecuteCmdlet()
        {
            if (this.Scope.Equals(PowerBIUserScope.Individual))
            {
                throw new NotImplementedException($"{CmdletVerb}-{CmdletName} is only supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }

            if (this.Scope == PowerBIUserScope.Organization)
            {
                this.Logger.WriteWarning($"Only preview workspaces are supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }

            IPowerBIClient client = this.CreateClient();

            var groupRestoreRequest = new GroupRestoreRequest { Name = this.Name, EmailAddress = this.UserPrincipalName };
            if (this.ParameterSetName.Equals(PropertiesParameterSetName))
            {
                var response = client.Groups.RestoreDeletedGroupAsAdmin(this.Id.ToString(), groupRestoreRequest);
                this.Logger.WriteObject(response);
            }
            else if (this.ParameterSetName.Equals(WorkspaceParameterSetName))
            {
                var response = client.Groups.RestoreDeletedGroupAsAdmin(this.Workspace.Id.ToString(), groupRestoreRequest);
                this.Logger.WriteObject(response);
            }
        }
    }
}