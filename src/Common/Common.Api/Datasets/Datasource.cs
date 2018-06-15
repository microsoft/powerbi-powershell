using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Datasource
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string DatasourceType { get; set; }
        public DatasourceConnectionDetails ConnectionDetails { get; set; }
        public string GatewayId { get; set; }
        public string DatasourceId { get; set; }

        public static implicit operator Datasource(PowerBI.Api.V2.Models.Datasource datasource)
        {
            return new Datasource
            {
                Name = datasource.Name,
                ConnectionString = datasource.ConnectionString,
                DatasourceType = datasource.DatasourceType,
                ConnectionDetails = (DatasourceConnectionDetails)datasource.ConnectionDetails,
                GatewayId = datasource.GatewayId,
                DatasourceId = datasource.DatasourceId
            };
        }
    }
}
