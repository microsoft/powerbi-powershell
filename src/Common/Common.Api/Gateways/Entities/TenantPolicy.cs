/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    [Flags]
    public enum TenantPolicy
    {
        [EnumMember]
        None = 0x0,

        [EnumMember]
        PersonalInstallRestricted = 0x1,

        [EnumMember]
        EnterpriseInstallRestricted = 0x2
    }
}
