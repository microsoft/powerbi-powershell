/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
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

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                try
                {
                    var workspaces = this.GetWorkspaces(client);
                    var matchedWorkspace = this.GetMatchingWorkspace(workspaces);
                    var response = client.Admin.GetPowerBIWorkspaceEncryptionStatus(matchedWorkspace.Id.ToString());

                    this.Logger.WriteObject(response);
                }
                catch (Exception ex)
                {
                    this.Logger.ThrowTerminatingError(ex);
                }
            }
        }

        private IEnumerable<Workspace> GetWorkspaces(IPowerBIApiClient client)
        {
            string nameFilter = $"name eq '{this.Name}'";
            var workspaces = client.Workspaces.GetWorkspacesAsAdmin(default, nameFilter, 1, default);
            if (workspaces == null)
            {
                throw new Exception("No workspaces are found");
            }

            return workspaces;
        }

        private Workspace GetMatchingWorkspace(IEnumerable<Workspace> workspaces)
        {
            var matchedWorkspace = workspaces.FirstOrDefault(
                    (workspace) => workspace.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            if (matchedWorkspace == default(Workspace))
            {
                throw new Exception("No matching workspaces are found");
            }

            return matchedWorkspace;
        }
    }
}
