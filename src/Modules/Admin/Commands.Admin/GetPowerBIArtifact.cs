/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Workspace>))]
    public class GetPowerBIArtifact : PowerBIGetCmdlet, IUserFirstSkip
    {
        public const string CmdletName = "PowerBIArtifact";
        public const string CmdletVerb = VerbsCommon.Get;
        
        public GetPowerBIArtifact() : base() { }

        public GetPowerBIArtifact(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameter set names
        private const string ListParameterSetName = "List";
        #endregion

        #region Parameters

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.First > 5000)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.First)} cannot be greater than 5000.");
            }
        }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                if (this.All.IsPresent)
                {
                    this.ExecuteCmdletWithAll();
                }
                else
                {
                    this.ExecuteCmdletWithFirstAndSkip();
                }
            }
        }

        private void ExecuteCmdletWithAll()
        {
            using (var client = this.CreateClient())
            {
                var workspaces = this.ExecuteCmdletWithAll(
                    (top, skip) => client.Workspaces.GetWorkspacesAsAdmin(expand: "reports,dashboards,datasets,dataflows,workbooks", top: top, skip: skip));

                this.Logger.WriteObject(workspaces, enumerateCollection: true);
            }
        }

        private void ExecuteCmdletWithFirstAndSkip()
        {
            using (var client = this.CreateClient())
            {
                bool defaultingFirst = false;
                if (this.First == default)
                {
                    this.First = 100;
                    defaultingFirst = true;
                }

                var workspaces = client.Workspaces.GetWorkspacesAsAdmin(
                    expand: "reports,dashboards,datasets,dataflows,workbooks", top: this.First, skip: this.Skip);

                if (defaultingFirst && workspaces.Count() == 100)
                {
                    this.Logger.WriteWarning("Defaulted to show top 100 workspaces. Use -First & -Skip or -All to retrieve more results.");
                }

                this.Logger.WriteObject(workspaces, enumerateCollection: true);
            }
        }
    }
}
