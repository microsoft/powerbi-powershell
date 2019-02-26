/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface IPowerBIConfigurationSettings
    {
        bool ForceDeviceCodeAuthentication { get; set; }

        bool ShowADALDebugMessages { get; set; }

        TimeSpan? HttpTimeout { get; set; }
    }
}
