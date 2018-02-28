/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions
{
    public enum PowerBIEnvironmentType
    {
#if DEBUG
        PPE = int.MaxValue,
#endif
        Public = 0
    }
}