/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public enum GroupUserAccessRight
    {
        None        = 0x0,
        Member      = 0x1,
        Admin       = 0x2,
        Contributor = 0x3,
        Viewer      = 0x4,
    }
}
