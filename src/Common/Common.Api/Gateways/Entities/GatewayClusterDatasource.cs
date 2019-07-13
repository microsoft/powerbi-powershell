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
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(Name = "clusterId", EmitDefaultValue = false)]
        public Guid ClusterId { get; set; }

        [DataMember(Name = "datasourceType", EmitDefaultValue = false)]
        public string DatasourceType { get; set; }

        [DataMember(Name = "connectionDetails", EmitDefaultValue = false)]
        public string ConnectionDetails { get; set; }

        [DataMember(Name = "credentialType", EmitDefaultValue = false)]
        public string CredentialType { get; set; }

        [DataMember(Name = "datasourceName", EmitDefaultValue = false)]
        public string DatasourceName { get; set; }

        [DataMember(Name = "users", EmitDefaultValue = false)]
        public IEnumerable<UserAccessRightEntry> Users { get; set; }

        [DataMember(Name = "datasourceErrorDetails", EmitDefaultValue = false)]
        public IEnumerable<DatasourceErrorDetails> DatasourceErrorDetails { get; set; }
    }
}
