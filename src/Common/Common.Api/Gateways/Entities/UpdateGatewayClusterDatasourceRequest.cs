/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class UpdateGatewayClusterDatasourceRequest
    {
        [DataMember(Name = "datasourceName", EmitDefaultValue = false)]
        public string DatasourceName { get; set; }

        [DataMember(Name = "datasourceAnnotation", EmitDefaultValue = false)]
        public string Annotation { get; set; }

        [DataMember(Name = "singleSignOnType", EmitDefaultValue = false)]
        public string SingleSignOnType { get; set; }
    }
}