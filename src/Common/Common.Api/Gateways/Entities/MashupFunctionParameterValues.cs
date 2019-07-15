/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class MashupFunctionParameterValues
    {
        [DataMember(Name = "name", Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "value", Order = 10)]
        public string Value { get; set; }

        [DataMember(Name = "type", Order = 20)]
        public string Type { get; set; }
    }
}