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
        private IPublicClientApplication AuthApplication;

        public async Task<IAccessToken> Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            IEnumerable<string> scopes = Constants.ApiScopes.Select(s => $"{environment.AzureADResource}/{s}");
            if (this.AuthApplication == null)
            {
                this.AuthApplication = PublicClientApplicationBuilder
                .Create(environment.AzureADClientId)
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .WithLogging((level, message, containsPii) => LoggingUtils.LogMsal(level, message, containsPii, logger))
                .WithRedirectUri(environment.AzureADRedirectAddress)
                .Build();
            }

            AuthenticationResult result = null;
            var accounts = await AuthApplication.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    result = await AuthApplication.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                    return result.ToIAccessToken();
                }
                catch (MsalUiRequiredException e)
                {
                    // ignore and fall through to aquire through device code
                }
            }

            DeviceCodeResult deviceCodeResult = null;
            result = await AuthApplication.AcquireTokenWithDeviceCode(scopes, r => { Console.WriteLine(r.Message); deviceCodeResult = r; return Task.FromResult(0); }).ExecuteAsync();

            return result.ToIAccessToken();
        }

        public async Task<IAccessToken> Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            await Task.Delay(0);
            // Not supported in .NET Core or DeviceCodeAuthentication - https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/issues/482
            throw new NotSupportedException("User and password authentication is not supported in .NET Core or with DeviceCode authentication.");
        }

        public async Task Challenge()
        {
            if (this.AuthApplication != null)
            {
                var accounts = (await this.AuthApplication.GetAccountsAsync()).ToList();
                while (accounts.Any())
                {
                    await this.AuthApplication.RemoveAsync(accounts.First());
                    accounts = (await this.AuthApplication.GetAccountsAsync()).ToList();
                }

                this.AuthApplication = null;
            }
        }
    }
}
