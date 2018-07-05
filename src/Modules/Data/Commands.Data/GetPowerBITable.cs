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
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DatasetIdParameterSetName)]
    [OutputType(typeof(IEnumerable<Table>))]
    public class GetPowerBITable : PowerBIClientCmdlet, IUserFirstSkip, IUserScope
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBITable";

        #region ParameterSets
        private const string DatasetIdParameterSetName = "DatasetId";
        private const string DatasetParameterSetName = "Dataset";
        #endregion

        #region Parameters
        [Parameter(Mandatory = true, ParameterSetName = DatasetIdParameterSetName)]
        public Guid DatasetId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = DatasetParameterSetName)]
        public Dataset Dataset { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Top")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public int? Skip { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public Workspace Workspace { get; set; }
        #endregion

        #region Constructors
        public GetPowerBITable() : base() { }

        public GetPowerBITable(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {           
            base.BeginProcessing();
            if (this.Scope.Equals(PowerBIUserScope.Organization))
            {
                throw new NotImplementedException($"{CmdletVerb}-{CmdletName} is only supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Individual)} is specified");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.Dataset != null)
            {
                this.DatasetId = this.Dataset.Id;
            }

            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }
            
            IEnumerable<Table> tables = null;
            using (var client = this.CreateClient())
            {
                if (this.WorkspaceId != default)
                {
                    tables = client.Datasets.GetTables(this.DatasetId, this.WorkspaceId);
                }
                else
                {
                    tables = client.Datasets.GetTables(this.DatasetId);
                }
            }

            if (!string.IsNullOrEmpty(this.Name))
            {
                tables = tables?.Where(d => d.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (this.Skip.HasValue)
            {
                tables = tables?.Skip(this.Skip.Value);
            }

            if (this.First.HasValue)
            {
                tables = tables?.Take(this.First.Value);
            }

            this.Logger.WriteObject(tables, true);
        }
    }
}
