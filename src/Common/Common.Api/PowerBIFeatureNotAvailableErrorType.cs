/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api
{
    [DataContract]
    public class PowerBIFeatureNotAvailableErrorType
    {
        [DataMember(Name = "error")]
        public PowerBIFeatureNotAvailableError Error { get; set; }
    }
}
