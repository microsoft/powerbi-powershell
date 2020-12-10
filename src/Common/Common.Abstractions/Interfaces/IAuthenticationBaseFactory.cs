/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Base Factory for Authentication (common among authentication factories).
    /// </summary>
    public interface IAuthenticationBaseFactory
    {
        /// <summary>
        /// Indicates the factory has been used once to authenticate; otherwise it is the first use of the factory.
        /// </summary>
        bool AuthenticatedOnce { get; }

        /// <summary>
        /// Revokes any previous authentication session.
        /// </summary>
        void Challenge();
    }
}
