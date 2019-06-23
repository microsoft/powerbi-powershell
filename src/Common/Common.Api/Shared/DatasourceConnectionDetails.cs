/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Shared
{
    public class DatasourceConnectionDetails
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Url { get; set; }

        public static implicit operator DatasourceConnectionDetails(PowerBI.Api.V2.Models.DatasourceConnectionDetails datasourceConnectionDetails)
        {
            if(datasourceConnectionDetails == null)
            {
                return null;
            }

            return new DatasourceConnectionDetails
            {
                Server = datasourceConnectionDetails.Server,
                Database = datasourceConnectionDetails.Database,
                Url = datasourceConnectionDetails.Url
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.DatasourceConnectionDetails(DatasourceConnectionDetails datasourceConnectionDetails)
        {
            if (datasourceConnectionDetails == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.DatasourceConnectionDetails
            {
                Server = datasourceConnectionDetails.Server,
                Database = datasourceConnectionDetails.Database,
                Url = datasourceConnectionDetails.Url
            };
        }
    }
}
