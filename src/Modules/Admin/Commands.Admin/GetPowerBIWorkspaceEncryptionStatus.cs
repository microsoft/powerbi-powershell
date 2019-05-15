/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = NameParameterSet)]
    [OutputType(typeof(IEnumerable<Dataset>))]
    public class GetPowerBIWorkspaceEncryptionStatus : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIWorkspaceEncryptionStatus";
        public const string CmdletVerb = VerbsCommon.Get;

        public GetPowerBIWorkspaceEncryptionStatus() : base() { }

        public GetPowerBIWorkspaceEncryptionStatus(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameter set names
        public const string NameParameterSet = "Name";
        public const string IdParameterSet = "Id";
        public const string WorkspaceParameterSet = "Workspace";
        #endregion

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSet)]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSet)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSet, ValueFromPipeline = true)]
        public Workspace Workspace { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                switch (ParameterSet)
                {
                    case NameParameterSet:
                        Workspace = this.GetWorkspace(client, Name);
                        if (Workspace == null)
                        {
                            // Return for test cases where no matching workspace was found
                            return;
                        }
                        Id = Workspace.Id;
                        break;
                    case IdParameterSet:
                        break;
                    case WorkspaceParameterSet:
                        Id = Workspace.Id;
                        break;
                }

                var response = client.Admin.GetPowerBIWorkspaceEncryptionStatus(Id);
                this.Logger.WriteObject(response, enumerateCollection: true);
            }
        }

        private Workspace GetWorkspace(IPowerBIApiClient client, string workspaceName)
        {
            string nameFilter = $"name eq '{workspaceName}'";
            var workspaces = client.Workspaces.GetWorkspacesAsAdmin(top: 1, filter: nameFilter);
            if (workspaces == null || !workspaces.Any())
            {
                this.Logger.ThrowTerminatingError("No matching workspace was found");
                return null;
            }

            return workspaces.Single();
        }
    }
}
