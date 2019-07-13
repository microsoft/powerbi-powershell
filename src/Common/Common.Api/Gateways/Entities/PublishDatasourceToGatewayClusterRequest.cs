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
    public sealed class PublishDatasourceToGatewayClusterRequest
    {
        [DataMember(Name = "datasourceType", EmitDefaultValue = false)]
        public string DatasourceType { get; set; }

        [DataMember(Name = "connectionDetails", EmitDefaultValue = false)]
        public string ConnectionDetails { get; set; }

        [DataMember(Name = "credentialDetails", EmitDefaultValue = false)]
        public IDictionary<Guid, DatasourceCredentialDetails> CredentialDetailsDictionary { get; set; }

        [DataMember(Name = "datasourceName", EmitDefaultValue = false)]
        public string DatasourceName { get; set; }

        [DataMember(Name = "singleSignOnType", EmitDefaultValue = false)]
        public string SingleSignOnType { get; set; }

        [DataMember(Name = "datasourceAnnotation", EmitDefaultValue = false)]
        public string Annotation { get; set; }

        [DataMember(Name = "mashupTestConnectionDetails", EmitDefaultValue = false)]
        public MashupTestConnectionDetails MashupTestConnectionDetails { get; set; }
    }
}