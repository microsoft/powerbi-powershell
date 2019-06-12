/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class GatewayClusterStatusResponse
    {
        [DataMember(Name = "clusterStatus")]
        public string ClusterStatus { get; set; }

        [DataMember(Name = "gatewayStaticCapabilities")]
        public string GatewayStaticCapabilities { get; set; }

        [DataMember(Name = "gatewayVersion")]
        public string GatewayVersion { get; set; }

        [DataMember(Name = "gatewayUpgradeState")]
        public string GatewayUpgradeState { get; set; }
    }
}
