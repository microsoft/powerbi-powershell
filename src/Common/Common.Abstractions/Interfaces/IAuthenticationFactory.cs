using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Factory for providing authentication to callers.
    /// </summary>
    public interface IAuthenticationFactory
    {
        /// <summary>
        /// Indicates the factory has been used once to authenticate; otherwise it is the first use of the factory.
        /// </summary>
        bool AuthenticatedOnce { get; }

        /// <summary>
        /// Authenticate given the set of parameters.
        /// </summary>
        /// <param name="environment">The IPowerBIEnvironment containing the connection information.</param>
        /// <param name="logger">The IPowerBILogger to log any messages for the user or telemetry.</param>
        /// <param name="settings">The IPowerBISettings to contain the settings to instruct the authentication to change its default behavior.</param>
        /// <param name="queryParameters">Query parameters to include with the request. This is optional.</param>
        /// <returns></returns>
        IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null);

        /// <summary>
        /// Revokes any previous authentication session.
        /// </summary>
        void Challenge();
    }
}