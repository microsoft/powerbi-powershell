/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Abstractions
{
    [DataContract]
    public class PowerBIFeatureNotAvailableError
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
