/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class ODataResponseGatewayCluster :  GatewayCluster
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }
    }
}
