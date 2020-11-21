/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface IPowerBIConfigurationSettings
    {
        bool ForceDeviceCodeAuthentication { get; }

        bool ShowMASLDebugMessages { get; }

        string DefaultClientId { get; }

        string DefaultRedirect { get; }
    }
}
