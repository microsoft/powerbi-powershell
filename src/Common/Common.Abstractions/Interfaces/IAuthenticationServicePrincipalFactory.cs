/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface IAuthenticationServicePrincipalFactory : IAuthenticationBaseFactory
    {
        /// <summary>
        /// Authenticates a service principal with username (appId\clientId) and password (app key).
        /// </summary>
        /// <param name="userName">Username (also called appId\clientId\objectId) of service principal.</param>
        /// <param name="password">Password for service principal (also called app key).</param>
        /// <param name="environment">The IPowerBIEnvironment containing the connection information.</param>
        /// <param name="logger">The IPowerBILogger to log any messages for the user or telemetry.</param>
        /// <param name="settings">The IPowerBISettings to contain the settings to instruct the authentication to change its default behavior.</param>
        /// <returns>An IAccessToken of an authenticated user.</returns>
        IAccessToken Authenticate(string userName, SecureString password, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings);

        /// <summary>
        /// Authenticates a service principal with clientId (appId) and certificate (with private key).
        /// </summary>
        /// <param name="clientId">The clientId (appId) of AAD application the certificate and service principal are associated to.</param>
        /// <param name="thumbprint">Thumbprint of certificate in CurrentUser or LocalMachien My certificate stores.</param>
        /// <param name="environment">The IPowerBIEnvironment containing the connection information.</param>
        /// <param name="logger">The IPowerBILogger to log any messages for the user or telemetry.</param>
        /// <param name="settings">The IPowerBISettings to contain the settings to instruct the authentication to change its default behavior.</param>
        /// <returns>An IAccessToken of an authenticated user.</returns>
        IAccessToken Authenticate(string clientId, string thumbprint, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings);
    }
}
