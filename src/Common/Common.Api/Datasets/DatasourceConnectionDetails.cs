using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class DatasourceConnectionDetails
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Url { get; set; }

        public static implicit operator DatasourceConnectionDetails(PowerBI.Api.V2.Models.DatasourceConnectionDetails datasourceConnectionDetails)
        {
            return new DatasourceConnectionDetails
            {
                Server = datasourceConnectionDetails.Server,
                Database = datasourceConnectionDetails.Database,
                Url = datasourceConnectionDetails.Url
            };
        }
    }
}
