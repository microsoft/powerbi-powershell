/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class UpdateGatewayInstallersRequest
    {
        [DataMember(Name = "ids")]
        public IEnumerable<string> Ids { get; set; } = new string[0];

        [DataMember(Name = "operation")]
        public OperationType Operation { get; set; }

        [DataMember(Name = "gatewayType")]
        public GatewayType GatewayType { get; set; }
    }
}
