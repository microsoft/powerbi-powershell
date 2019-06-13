/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api.Capacities
{
    public class CapacityClient : PowerBIEntityClient, ICapacityClient
    {
        public CapacityClient(IPowerBIClient client) : base(client) { }

        public IEnumerable<Capacity> GetCapacities()
        {
            return this.Client.Capacities.GetCapacities()?.Value
                    .Select(capacity => (Capacity)capacity);
        }

        public IEnumerable<Capacity> GetCapacitiesAsAdmin(string expand = null)
        {
            return this.Client.Admin.GetCapacitiesAsAdmin(expand)?.Value
                    .Select(capacity => (Capacity)capacity);
        }
    }
}
