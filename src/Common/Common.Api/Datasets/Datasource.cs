/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

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
            if(datasource == null)
            {
                return null;
            }

            return new Datasource
            {
                Name = datasource.Name,
                ConnectionString = datasource.ConnectionString,
                DatasourceType = datasource.DatasourceType,
                ConnectionDetails = datasource.ConnectionDetails,
                GatewayId = datasource.GatewayId,
                DatasourceId = datasource.DatasourceId
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Datasource(Datasource datasource)
        {
            if (datasource == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Datasource
            {
                Name = datasource.Name,
                ConnectionString = datasource.ConnectionString,
                DatasourceType = datasource.DatasourceType,
                ConnectionDetails = datasource.ConnectionDetails,
                GatewayId = datasource.GatewayId,
                DatasourceId = datasource.DatasourceId
            };
        }
    }
}
