using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Dataset>))]
    public class GetPowerBIDataset : PowerBIClientCmdlet, IUserScope, IUserFilter, IUserFirstSkip, IUserId
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIDataset";

        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";

        public GetPowerBIDataset() : base() { }

        public GetPowerBIDataset(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("DatasetId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string Filter { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Individual && !string.IsNullOrEmpty(this.Filter))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Filter)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.ParameterSet.Equals(IdParameterSetName))
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.ParameterSet.Equals(NameParameterSetName))
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            IEnumerable<Dataset> datasets = null;
            using (var client = this.CreateClient())
            {
                if(this.WorkspaceId != default)
                {
                    datasets = this.Scope == PowerBIUserScope.Individual ?
                        client.Datasets.GetDatasetsForWorkspace(this.WorkspaceId) :
                        client.Datasets.GetDatasetsAsAdminForWorkspace(this.WorkspaceId, filter: this.Filter, top: this.First, skip: this.Skip);
                }
                else
                {
                    datasets = this.Scope == PowerBIUserScope.Individual ?
                        client.Datasets.GetDatasets() :
                        client.Datasets.GetDatasetsAsAdmin(filter: this.Filter, top: this.First, skip: this.Skip);
                }
            }

            if (this.Scope == PowerBIUserScope.Individual)
            {
                if (this.Id != default)
                {
                    datasets = datasets?.Where(r => this.Id == new Guid(r.Id)).ToList();
                }

                if (!string.IsNullOrEmpty(this.Name))
                {
                    datasets?.Where(r => r.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (this.Skip.HasValue)
                {
                    datasets = datasets?.Skip(this.Skip.Value);
                }

                if (this.First.HasValue)
                {
                    datasets = datasets?.Take(this.First.Value);
                }
            }

            this.Logger.WriteObject(datasets, true);
        }
    }
}
