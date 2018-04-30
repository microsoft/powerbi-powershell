/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api
{
    /// <summary>
    /// The base class for entity-specific classes that wrap their equivalent in the Power BI SDK.
    /// </summary>
    /// <remarks>
    /// This class is abstract despite having no behavior to override because it should not be initialized
    /// and it allows the logic for the Power BI SDK client initialization to be defined once.
    /// </remarks>
    public abstract class PowerBIEntityClient
    {
        protected IPowerBIClient Client;

        public PowerBIEntityClient(IPowerBIClient client) => this.Client = client ?? throw new ArgumentNullException("client");
    }
}
