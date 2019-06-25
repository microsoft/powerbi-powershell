/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Dataflow>))]
    public class GetPowerBIDataflow : PowerBIClientCmdlet, IUserId, IUserScope, IUserFilter, IUserFirstSkip
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIDataflow";

        #region ParameterSets
        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";
        private const string ObjectIdParameterSetName = "ObjectAndId";
        private const string ObjectNameParameterSetName = "ObjectAndName";
        private const string ObjectListParameterSetName = "ObjectAndList";
        #endregion

        #region Parameters
        [Alias("DataflowId")]
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string Filter { get; set; }

        [Alias("Top")]
        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = ObjectListParameterSetName)]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = ObjectListParameterSetName)]
        public int? Skip { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = NameParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectListParameterSetName, ValueFromPipeline = true)]
        public Workspace Workspace { get; set; }
        #endregion

        #region Constructors
        public GetPowerBIDataflow() : base() { }

        public GetPowerBIDataflow(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Individual && !string.IsNullOrEmpty(this.Filter))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Filter)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }

            if (this.Scope == PowerBIUserScope.Individual && this.WorkspaceId == null && this.Workspace == null)
            {
                this.Logger.ThrowTerminatingError($"When -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Individual)}, {nameof(this.Workspace)} or {nameof(this.WorkspaceId)} must be applied");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            if (this.Id != default)
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.Name != default)
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            IEnumerable<Dataflow> dataflows = null;
            using (var client = this.CreateClient())
            {
                if (this.WorkspaceId != default)
                {
                    dataflows = this.Scope == PowerBIUserScope.Individual ?
                        client.Dataflows.GetDataflows(this.WorkspaceId) :
                        client.Dataflows.GetDataflowsAsAdminForWorkspace(this.WorkspaceId, filter: this.Filter, top: this.First, skip: this.Skip);
                }
                else
                {
                    dataflows = this.Scope == PowerBIUserScope.Organization ?
                        client.Dataflows.GetDataflowsAsAdmin(filter: this.Filter, top: this.First, skip: this.Skip) :
                        null;
                }
            }

            if (this.Scope == PowerBIUserScope.Individual)
            {
                if (this.Id != default)
                {
                    dataflows = dataflows?.Where(d => this.Id == d.Id);
                }

                if (!string.IsNullOrEmpty(this.Name))
                {
                    dataflows?.Where(d => d.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (this.Skip.HasValue)
                {
                    dataflows = dataflows?.Skip(this.Skip.Value);
                }

                if (this.First.HasValue)
                {
                    dataflows = dataflows?.Take(this.First.Value);
                }
            }

            this.Logger.WriteObject(dataflows, true);
        }
    }
}