/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class UpdateGatewayPolicyRequest
    {
        [DataMember(IsRequired = false, Name = "resourceGatewayInstallPolicy")]
        public PolicyType ResourceGatewayInstallPolicy { get; set; }

        [DataMember(IsRequired = false, Name = "personalGatewayInstallPolicy")]
        public PolicyType PersonalGatewayInstallPolicy { get; set; }
    }
}
