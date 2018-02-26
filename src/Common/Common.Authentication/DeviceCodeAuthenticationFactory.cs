using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class DeviceCodeAuthenticationFactory : IAuthenticationFactory
    {
        private static bool authenticatedOnce = false;
        public bool AuthenticatedOnce { get => authenticatedOnce; }

        private static object tokenCacheLock = new object();

        private static TokenCache Cache { get; set; }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            if (Cache == null)
            {
                lock (tokenCacheLock)
                {
                    if (Cache == null)
                    {
                        Cache = new TokenCache();
                    }
                }
            }

            var context = new AuthenticationContext(environment.AzureADAuthority, Cache);
            string queryParamString = queryParameters.ToQueryParameterString();

            AuthenticationResult token = null;
            if (this.AuthenticatedOnce)
            {
                try
                {
                    token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
                    return token.ToIAccessToken();
                }
                catch (AdalSilentTokenAcquisitionException)
                {
                    // ignore and fall through to aquire through device code
                }
            }

            var deviceCodeResult = context.AcquireDeviceCodeAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            logger.WriteHost("You need to sign in.");
            logger.WriteHost(deviceCodeResult.Message + Environment.NewLine);

            token = context.AcquireTokenByDeviceCodeAsync(deviceCodeResult).Result;
            authenticatedOnce = true;
            return token.ToIAccessToken();
        }

        public void Challenge()
        {
            if (Cache != null)
            {
                lock (tokenCacheLock)
                {
                    if (Cache != null)
                    {
                        authenticatedOnce = false;
                        Cache = null;
                    }
                }
            }
        }
    }
}