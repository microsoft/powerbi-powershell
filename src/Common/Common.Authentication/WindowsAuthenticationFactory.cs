/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Abstractions.Utilities;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class WindowsAuthenticationFactory : IAuthenticationUserFactory
    {
        private IPublicClientApplication AuthApplication;

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            return HandleAuthentication(environment, logger, settings, queryParameters);
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            return HandleAuthentication(environment, logger, settings, null, userName, password);
        }

        private IAccessToken HandleAuthentication(
            IPowerBIEnvironment environment,
            IPowerBILogger logger,
            IPowerBISettings settings,
            IDictionary<string, string> queryParameters,
            string userName = null,
            SecureString password = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("Authenticator only works on Windows");
            }

            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            if (this.AuthApplication == null)
            {
                Console.WriteLine("Init auth app");
                this.AuthApplication = PublicClientApplicationBuilder
                    .Create(environment.AzureADClientId)
                    .WithAuthority(environment.AzureADAuthority)
                    .WithLogging((level, message, containsPii) => LoggingUtils.LogMsal(level, message, containsPii, logger))
                    .WithExtraQueryParameters(queryParameters)
                    .Build();
            }

            AuthenticationResult result = null;

            try
            {
                var accounts = this.AuthApplication.GetAccountsAsync().Result;
                if (accounts.Any())
                {
                    // This indicates there's token in cache
                    Console.WriteLine("AcquireTokenSilent: " + accounts.First().Username);
                    result = this.AuthApplication.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync().Result;
                }
                else
                {
                    if (!string.IsNullOrEmpty(userName) && password != null && password.Length > 0)
                    {
                        Console.WriteLine("AcquireTokenByUsernamePassword");
                        // https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/wiki/Acquiring-tokens-with-username-and-password
                        result = this.AuthApplication.AcquireTokenByUsernamePassword(scopes, userName, password).ExecuteAsync().Result;
                    }
                    else
                    {
                        Console.WriteLine("AcquireTokenInteractive");
                        result = this.AuthApplication.AcquireTokenInteractive(scopes).ExecuteAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AuthenticationException($"Error Acquiring Token:{System.Environment.NewLine}{ex}");
            }

            if (result != null)
            {
                return result.ToIAccessToken();
                // Use the token
            }
            else
            {
                throw new AuthenticationException("Failed to acquire token");
            }
        }

        public async Task Challenge(ICollection<IPowerBIEnvironment> environments)
        {
            foreach (var environment in environments)
            {
                var app = PublicClientApplicationBuilder
                    .Create(environment.AzureADClientId)
                    .WithAuthority(environment.AzureADAuthority)
                    .Build();

                var accounts = await app.GetAccountsAsync();
                while (accounts.Any())
                {
                    Console.WriteLine("Challenge:" + accounts.FirstOrDefault()?.Username);
                    await app.RemoveAsync(accounts.First());
                    accounts = await app.GetAccountsAsync();
                }
            }
        }

    }
}