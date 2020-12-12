/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Base Factory for Authentication (common among authentication factories).
    /// </summary>
    public interface IAuthenticationBaseFactory
    {
        /// <summary>
        /// Revokes any previous authentication session.
        /// </summary>
        Task Challenge(ICollection<IPowerBIEnvironment> environments);
    }
}
