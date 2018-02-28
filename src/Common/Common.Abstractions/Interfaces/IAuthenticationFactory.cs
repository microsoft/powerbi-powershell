/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Security;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Factory for providing authentication to callers.
    /// </summary>
    public interface IAuthenticationFactory : IAuthenticationUserFactory, IAuthenticationServicePrincipalFactory
    {
        /// <summary>
        /// Authenticates based on configuration stored in profile.
        /// </summary>
        /// <param name="profile">The IPowerBIProfile to determine how to authenticate.</param>
        /// <param name="logger">The IPowerBILogger to log any messages for the user or telemetry.</param>
        /// <param name="settings">The IPowerBISettings to contain the settings to instruct the authentication to change its default behavior.</param>
        /// <param name="queryParameters">Query parameters to include with the request. This is optional.</param>
        /// <returns>An IAccessToken of an authenticated user.</returns>
        IAccessToken Authenticate(IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null);
    }
}