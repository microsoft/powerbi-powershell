/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Extensible settings.
    /// </summary>
    public interface IExtensibleSettings
    {
        /// <summary>
        /// Dictionary to store settings.
        /// </summary>
        IDictionary<string, string> Settings { get; }
    }
}