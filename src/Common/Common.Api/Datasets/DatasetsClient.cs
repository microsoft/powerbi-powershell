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
            throw new NotImplementedException();
        }

        public IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return Client.Datasets.GetDatasetsAsAdmin(filter: filter, top: top, skip: skip).Value?.Select(x => (Dataset)x);
        }
    }
}
