/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Capacities
{
    public interface ICapacityClient
    {
        IEnumerable<Capacity> GetCapacities();

        IEnumerable<Capacity> GetCapacitiesAsAdmin(string expand = default);
    }
}
