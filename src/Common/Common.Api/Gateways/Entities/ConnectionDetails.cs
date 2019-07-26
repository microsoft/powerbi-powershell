/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public abstract class ConnectionDetails<T> where T : ConnectionDetails<T>
    {
        public abstract T Normalize();
    }
}
