/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class GatewayClusterDatasource
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "clusterId")]
        public Guid ClusterId { get; set; }

        [DataMember(Name = "datasourceType")]
        public string DatasourceType { get; set; }

        [DataMember(Name = "connectionDetails")]
        public string ConnectionDetails { get; set; }

        [DataMember(Name = "credentialType")]
        public string CredentialType { get; set; }

        [DataMember(Name = "datasourceName")]
        public string DatasourceName { get; set; }

        [DataMember(Name = "users")]
        public IEnumerable<DatasourceUser> Users { get; set; }

        [DataMember(Name = "datasourceErrorDetails")]
        public IEnumerable<DatasourceErrorDetails> DatasourceErrorDetails { get; set; }
    }
}
