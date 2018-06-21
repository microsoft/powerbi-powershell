/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Dashboard>))]
    public class GetPowerBIDashboard : PowerBIClientCmdlet, IUserScope, IUserFilter, IUserId, IUserFirstSkip
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIDashboard";

        #region ParameterSets
        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";
        private const string ObjectIdParameterSetName = "ObjectAndId";
        private const string ObjectNameParameterSetName = "ObjectAndName";
        private const string ObjectListParameterSetName = "ObjectAndList";
        #endregion

        #region Parameters
        [Alias("ImportId")]
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = ObjectListParameterSetName)]
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
        public GetPowerBIDashboard() : base() { }

        public GetPowerBIDashboard(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Individual && !string.IsNullOrEmpty(this.Filter))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Filter)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}", ErrorCategory.InvalidArgument);
            }
        }

        public override void ExecuteCmdlet()
        {
            if(this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            if (this.ParameterSet.Equals(IdParameterSetName))
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.ParameterSet.Equals(NameParameterSetName))
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            IEnumerable<Dashboard> dashboards = null;
            using (var client = this.CreateClient())
            {
                if(this.WorkspaceId != default)
                {
                    dashboards = this.Scope == PowerBIUserScope.Individual ? 
                        client.Reports.GetDashboardsForWorkspace(workspaceId: this.WorkspaceId) :
                        client.Reports.GetDashboardsAsAdminForWorkspace(workspaceId: this.WorkspaceId, filter: this.Filter, top: this.First, skip: this.Skip);
                }
                else
                {
                    dashboards = this.Scope == PowerBIUserScope.Individual ?
                        client.Reports.GetDashboards() :
                        client.Reports.GetDashboardsAsAdmin(filter: this.Filter, top: this.First, skip: this.Skip);
                }
            }

            if(this.Scope == PowerBIUserScope.Individual)
            {
                if (this.Id != default)
                {
                    dashboards = dashboards?.Where(d => this.Id == d.Id);
                }

                if (!string.IsNullOrEmpty(this.Name))
                {
                    dashboards = dashboards?.Where(d => d.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (this.Skip.HasValue)
                {
                    dashboards = dashboards?.Skip(this.Skip.Value);
                }

                if (this.First.HasValue)
                {
                    dashboards = dashboards?.Take(this.First.Value);
                }
            }

            this.Logger.WriteObject(dashboards, true);
        }
    }
}
