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
        [DataMember(Name = "datasourceType")]
        public string DatasourceType { get; set; }

        [DataMember(Name = "connectionDetails")]
        public string ConnectionDetails { get; set; }

        [DataMember(Name = "credentialDetails")]
        public IDictionary<Guid, DatasourceCredentialDetails> CredentialDetailsDictionary { get; set; }

        [DataMember(Name = "datasourceName")]
        public string DatasourceName { get; set; }

        [DataMember(Name = "singleSignOnType")]
        public string SingleSignOnType { get; set; }

        [DataMember(Name = "datasourceAnnotation")]
        public string Annotation { get; set; }
    }
}