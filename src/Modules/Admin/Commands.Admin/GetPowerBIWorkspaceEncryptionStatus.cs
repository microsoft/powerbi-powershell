/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Dataset>))]
    public class GetPowerBIWorkspaceEncryptionStatus : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIWorkspaceEncryptionStatus";
        public const string CmdletVerb = VerbsCommon.Get;

        public GetPowerBIWorkspaceEncryptionStatus() : base() { }

        public GetPowerBIWorkspaceEncryptionStatus(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            this.Logger.WriteWarning(Constants.PrivatePreviewWarning);
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var workspace = this.GetWorkspace(client);
                if (workspace == null)
                {
                    // Return for test cases where no matching workspace was found
                    return;
                }
                
                var response = client.Admin.GetPowerBIWorkspaceEncryptionStatus(workspace.Id.ToString());
                this.Logger.WriteObject(response, enumerateCollection: true);
            }
        }

        private Workspace GetWorkspace(IPowerBIApiClient client)
        {
            string nameFilter = $"name eq '{this.Name}'";
            var workspaces = client.Workspaces.GetWorkspacesAsAdmin(default, nameFilter, 1, default);
            if (workspaces == null || !workspaces.Any())
            {
                this.Logger.ThrowTerminatingError("No matching workspace was found");
                return null;
            }

            return workspaces.Single();
        }
    }
}
