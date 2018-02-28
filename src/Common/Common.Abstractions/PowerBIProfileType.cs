/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions
{
    /// <summary>
    /// Types of PowerBI profiles.
    /// </summary>
    public enum PowerBIProfileType
    {
        User = 0,
        ServicePrincipal = 1,
        Certificate = 2
    }
}