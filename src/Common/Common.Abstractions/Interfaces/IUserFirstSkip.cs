/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface IUserFirstSkip
    {
        int? First { get; set; }

        int? Skip { get; set; }
    }
}
