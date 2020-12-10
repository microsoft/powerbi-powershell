/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class DeviceCodeAuthenticationFactory : IAuthenticationUserFactory
    {
        private static bool authenticatedOnce = false;
        public bool AuthenticatedOnce { get => authenticatedOnce; }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            IPublicClientApplication app = PublicClientApplicationBuilder
                .Create(environment.AzureADClientId)
                .WithAuthority(environment.AzureADAuthority)
                .WithDebugLoggingCallback(withDefaultPlatformLoggingEnabled: settings.Settings.ShowMSALDebugMessages)
                .Build();
            AuthenticationResult result = null;
            var accounts = app.GetAccountsAsync().Result;

            AuthenticationResult token = null;
            if (this.AuthenticatedOnce)
            {
                try
                {
                    result = app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync().Result;
                    return token.ToIAccessToken();
                }
                catch (MsalUiRequiredException)
                {
                    // ignore and fall through to aquire through device code
                }
            }

            DeviceCodeResult deviceCodeResult = null;
            result = app.AcquireTokenWithDeviceCode(scopes, r => { deviceCodeResult = r;  return Task.FromResult(0); }).ExecuteAsync().Result;
            logger.WriteHost("You need to sign in.");
            logger.WriteHost(deviceCodeResult?.Message + Environment.NewLine);

            authenticatedOnce = true;
            return result.ToIAccessToken();
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            // Not supported in .NET Core or DeviceCodeAuthentication - https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/issues/482
            throw new NotSupportedException("User and password authentication is not supported in .NET Core or with DeviceCode authentication.");
        }

        public void Challenge()
        {
            authenticatedOnce = false;
        }
    }
}
