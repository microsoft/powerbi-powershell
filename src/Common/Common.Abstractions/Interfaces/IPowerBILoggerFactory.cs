/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Factory for creating PowerBI loggers.
    /// </summary>
    public interface IPowerBILoggerFactory
    {
        /// <summary>
        /// Create a PowerBI logger for a given PowerShell cmdlet.
        /// </summary>
        /// <param name="cmdlet">Cmdlet to use inside the logger.</param>
        /// <returns>PowerBI logger.</returns>
        IPowerBILogger CreateLogger(PSCmdlet cmdlet);
    }
}