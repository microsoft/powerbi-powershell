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
    [Cmdlet(CmdletVerb, CmdletName)]
    [Alias("New-PowerBIGroup")]
    [OutputType(typeof(Workspace))]
    public class NewPowerBIWorkspace : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.New;

        #region Constructors
        public NewPowerBIWorkspace() : base() { }

        public NewPowerBIWorkspace(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("WorkspaceType")]
        public NewWorkspaceType Type { get; set; } = NewWorkspaceType.Workspace;

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var result = client.Workspaces.NewWorkspaceAsUser(this.Name, this.Type == NewWorkspaceType.Workspace);
                this.Logger.WriteObject(result, true);
            }
        }
    }

    public enum NewWorkspaceType
    {
        Workspace,
        Group
    }
}
