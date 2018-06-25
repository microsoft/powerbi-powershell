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
    [OutputType(typeof(IEnumerable<Report>))]
    public class GetPowerBIReport : PowerBIClientCmdlet, IUserScope, IUserFilter, IUserId
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIReport";

        #region ParameterSets
        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";
        private const string ObjectIdParameterSetName = "ObjectAndId";
        private const string ObjectNameParameterSetName = "ObjectAndName";
        private const string ObjectListParameterSetName = "ObjectAndList";
        #endregion

        #region Parameters
        [Alias("ReportId")]
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
        public GetPowerBIReport() : base() { }

        public GetPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if(this.Scope == PowerBIUserScope.Individual && !string.IsNullOrEmpty(this.Filter))
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

            if (this.Id != default)
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.Name != default)
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            IEnumerable<Report> reports = null;
            using (var client = this.CreateClient())
            {
                if(this.WorkspaceId != default)
                {
                    reports = this.Scope == PowerBIUserScope.Individual ?
                        client.Reports.GetReportsForWorkspace(this.WorkspaceId) :
                        client.Reports.GetReportsAsAdminForWorkspace(this.WorkspaceId, filter: this.Filter, top: this.First, skip: this.Skip);
                }
                else
                {
                    reports = this.Scope == PowerBIUserScope.Individual ?
                        client.Reports.GetReports() :
                        client.Reports.GetReportsAsAdmin(filter: this.Filter, top: this.First, skip: this.Skip);
                } 
            }

            // Bug in OData filter for ID, workaround is to use LINQ
            if (this.Id != default)
            {
                reports = reports?.Where(r => this.Id == r.Id);
            }

            if (this.Scope == PowerBIUserScope.Individual)
            {
                if(!string.IsNullOrEmpty(this.Name))
                {
                    reports = reports?.Where(r => r.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
                }

                if(this.Skip.HasValue)
                {
                    reports = reports?.Skip(this.Skip.Value);
                }

                if(this.First.HasValue)
                {
                    reports = reports?.Take(this.First.Value);
                }
            }

            this.Logger.WriteObject(reports, true);
        }
    }
}
