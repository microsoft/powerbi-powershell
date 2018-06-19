using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerBI.Api.V2;
using System.Linq;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class DatasetsClient : PowerBIEntityClient, IDatasetsClient
    {
        public DatasetsClient(IPowerBIClient client) : base(client)
        {
        }

        public IEnumerable<Dataset> GetDatasets()
        {
            return this.Client.Datasets.GetDatasets().Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(filter: filter, top: top, skip: skip).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Datasource> GetDatasources(Guid datasetId, Guid? workspaceId = default)
        {
            var result = workspaceId.HasValue ? this.Client.Datasets.GetDatasources(groupId: workspaceId.ToString(), datasetKey: datasetId.ToString()) : this.Client.Datasets.GetDatasources(datasetKey: workspaceId.ToString());
            return result.Value?.Select(x => (Datasource)x);

        }

        public IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid datasetId)
        {
            return this.Client.Datasets.GetDatasourcesAsAdmin(datasetId.ToString()).Value?.Select(x => (Datasource)x);
        }
    }
}
