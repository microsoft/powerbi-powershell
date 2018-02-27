using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface IAuthenticationUserFactory : IAuthenticationBaseFactory
    {
        /// <summary>
        /// Authenticate a interactive user given the set of parameters.
        /// </summary>
        /// <param name="environment">The IPowerBIEnvironment containing the connection information.</param>
        /// <param name="logger">The IPowerBILogger to log any messages for the user or telemetry.</param>
        /// <param name="settings">The IPowerBISettings to contain the settings to instruct the authentication to change its default behavior.</param>
        /// <param name="queryParameters">Query parameters to include with the request. This is optional.</param>
        /// <returns>An IAccessToken of an authenticated user.</returns>
        IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null);
    }
}
