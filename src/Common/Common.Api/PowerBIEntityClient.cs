/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api
{
    public abstract class PowerBIEntityClient
    {
        protected IPowerBIClient Client;

        public PowerBIEntityClient(IPowerBIClient client) => this.Client = client ?? throw new ArgumentNullException("client");
    }
}
