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
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.PowerBI.Common.Api.Shared;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Datasource>))]
    public class GetPowerBIDataflowDatasource : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIDataflowDatasource";

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
        [Parameter(Mandatory = true, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public Guid DataflowId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectListParameterSetName, ValueFromPipeline = true)]
        public Dataflow Dataflow { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }

        [Alias("DatasourceId")]
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName)]
        public Guid Id { get; set; }

        [Alias("DatasoureName")]
        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;
        #endregion

        #region Constructors
        public GetPowerBIDataflowDatasource() : base() { }

        public GetPowerBIDataflowDatasource(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope != PowerBIUserScope.Organization && this.WorkspaceId == default)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.WorkspaceId)} must be applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Individual)}");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.Dataflow != null)
            {
                this.DataflowId = this.Dataflow.Id;
            }

            IEnumerable<Datasource> datasources = null;
            using (var client = this.CreateClient())
            {
                if (this.WorkspaceId != default)
                {
                    datasources = this.Scope == PowerBIUserScope.Organization ?
                        client.Dataflows.GetDataflowDatasourcesAsAdmin(this.DataflowId) : 
                        client.Dataflows.GetDataflowDatasources(this.WorkspaceId, this.DataflowId);
                }
                else
                {
                    datasources = this.Scope == PowerBIUserScope.Organization ?
                        client.Dataflows.GetDataflowDatasourcesAsAdmin(this.DataflowId) :
                        null;
                }
            }

            if (this.Id != default)
            {
                datasources = datasources.Where(d => this.Id == d.DatasourceId);
            }

            if (!string.IsNullOrEmpty(this.Name))
            {
                datasources = datasources.Where(d => d.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
            }

            this.Logger.WriteObject(datasources, true);
        }
    }
}
