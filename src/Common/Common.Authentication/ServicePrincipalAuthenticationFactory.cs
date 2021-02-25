/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class ServicePrincipalAuthenticationFactory : IAuthenticationServicePrincipalFactory
    {
        private IConfidentialClientApplication AuthApplicationSecret;
        private IConfidentialClientApplication AuthApplicationCert;

        public async Task<IAccessToken> Authenticate(string clientId, SecureString clientSecret, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
 
            if (this.AuthApplicationSecret == null)
            {
                this.AuthApplicationSecret = ConfidentialClientApplicationBuilder
                   .Create(environment.AzureADClientId)
                   .WithAuthority(environment.AzureADAuthority)
                   .WithClientId(clientId)
                   .WithClientSecret(clientSecret.SecureStringToString())
                   .WithLogging((level, message, containsPii) => LoggingUtils.LogMsal(level, message, containsPii, logger))
                   .Build();
            }

            AuthenticationResult result = null;

            try
            {
                var accounts = await this.AuthApplicationSecret.GetAccountsAsync();
                if (accounts.Any())
                {
                    // This indicates there's token in cache
                    result = await this.AuthApplicationSecret.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                }
                else
                {
                    result = await this.AuthApplicationSecret.AcquireTokenForClient(scopes).ExecuteAsync();
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

        public async Task<IAccessToken> Authenticate(string clientId, string thumbprint, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            var certificate = FindCertificate(thumbprint);
            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };

            if (this.AuthApplicationCert == null)
            {
                this.AuthApplicationCert = ConfidentialClientApplicationBuilder
                   .Create(environment.AzureADClientId)
                   .WithAuthority(environment.AzureADAuthority)
                   .WithClientId(clientId)
                   .WithCertificate(certificate)
                   .WithLogging((level, message, containsPii) => LoggingUtils.LogMsal(level, message, containsPii, logger))
                   .Build();
            }

            AuthenticationResult result = null;

            try
            {
                var accounts = await this.AuthApplicationCert.GetAccountsAsync();
                if (accounts.Any())
                {
                    // This indicates there's token in cache
                    result = await this.AuthApplicationCert.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                }
                else
                {
                    result = await this.AuthApplicationCert.AcquireTokenForClient(scopes).ExecuteAsync();
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

        private static X509Certificate2 FindCertificate(string thumbprint)
        {
            X509Certificate2Collection certificates;
            if (TryFindCertificatesInLocation(thumbprint, StoreLocation.CurrentUser, out certificates) ||
                    TryFindCertificatesInLocation(thumbprint, StoreLocation.LocalMachine, out certificates))
            {
                return certificates[0];
            }

            throw new ArgumentException($"Unable to find a certificate with thumbprint '{thumbprint}' in CurrentUser or LocalMachine certificate stores", nameof(thumbprint));
        }

        private static bool TryFindCertificatesInLocation(string thumbprint, StoreLocation location, out X509Certificate2Collection certificates)
        {
            using (var store = new X509Store(StoreName.My, location))
            {
                store.Open(OpenFlags.ReadOnly);
                certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            }

            return certificates.Count > 0;
        }

        public async Task Challenge()
        {
            if (this.AuthApplicationSecret != null)
            {
                var accounts = (await this.AuthApplicationSecret.GetAccountsAsync()).ToList();
                while (accounts.Any())
                {
                    await this.AuthApplicationSecret.RemoveAsync(accounts.First());
                    accounts = (await this.AuthApplicationSecret.GetAccountsAsync()).ToList();
                }

                this.AuthApplicationSecret = null;
            }

            if (this.AuthApplicationCert != null)
            {
                var accounts = (await this.AuthApplicationSecret.GetAccountsAsync()).ToList();
                while (accounts.Any())
                {
                    await this.AuthApplicationSecret.RemoveAsync(accounts.First());
                    accounts = (await this.AuthApplicationSecret.GetAccountsAsync()).ToList();
                }

                this.AuthApplicationCert = null;
            }
        }
    }
}