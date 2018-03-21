/*
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
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = PropertiesParameterSetName)]
    public class SetPowerBIWorkspace : PowerBICmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Set;

        private const string PropertiesParameterSetName = "Properties";
        private const string WorkspaceParameterSetName = "Workspace";

        #region Parameters

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = WorkspaceParameterSetName)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true, ParameterSetName = PropertiesParameterSetName)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        public string Description { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName)]
        public Group Workspace { get; set; }

        #endregion

        protected override void ExecuteCmdlet()
        {
            if (this.Scope.Equals(PowerBIUserScope.Individual))
            {
                throw new NotImplementedException();
            }

            PowerBIClient client = null;
            var token = this.Authenticator.Authenticate(this.Profile, this.Logger, this.Settings);
            if (Uri.TryCreate(this.Profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                client = new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken));
            }
            else
            {
                client = new PowerBIClient(new TokenCredentials(token.AccessToken));
            }

            if (this.ParameterSetName.Equals(PropertiesParameterSetName))
            {
                var updatedGroup = new Group { Name = this.Name, Description = this.Description };
                var response = client.Groups.UpdateGroupAsAdmin(this.Id.ToString(), updatedGroup);
                this.Logger.WriteObject(response);
            }
            else if (this.ParameterSetName.Equals(WorkspaceParameterSetName))
            {
                var response = client.Groups.UpdateGroupAsAdmin(this.Workspace.Id.ToString(), this.Workspace);
                this.Logger.WriteObject(response);
            }
        }
    }
}
