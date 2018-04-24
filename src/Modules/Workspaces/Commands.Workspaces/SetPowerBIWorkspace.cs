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
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = PropertiesParameterSetName)]
    [Alias("Set-PowerBIGroup")]
    public class SetPowerBIWorkspace : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Set;

        private const string PropertiesParameterSetName = "Properties";
        private const string WorkspaceParameterSetName = "Workspace";

        public SetPowerBIWorkspace() : base() { }

        public SetPowerBIWorkspace(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = WorkspaceParameterSetName)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true, ParameterSetName = PropertiesParameterSetName, ValueFromPipelineByPropertyName = true)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = PropertiesParameterSetName)]
        public string Description { get; set; }

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
                this.Logger.WriteWarning($"Only preview workspaces are supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }
        }

        public override void ExecuteCmdlet()
        {
            var client = this.CreateClient();

            if (this.ParameterSetName.Equals(PropertiesParameterSetName))
            {
                var updatedProperties = new Workspace { Name = this.Name, Description = this.Description };
                var response = client.Workspaces.UpdateWorkspaceAsAdmin(this.Id, updatedProperties);
                this.Logger.WriteObject(response);
            }
            else if (this.ParameterSetName.Equals(WorkspaceParameterSetName))
            {
                // The API will throw 400 saying that it "Cannot apply PATCH to navigation property users" if we don't null this property out
                this.Workspace.Users = null;
                var response = client.Workspaces.UpdateWorkspaceAsAdmin(this.Workspace.Id, this.Workspace);
                this.Logger.WriteObject(response);
            }
        }
    }
}
