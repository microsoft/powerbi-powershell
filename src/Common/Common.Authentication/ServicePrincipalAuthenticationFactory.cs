/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class ServicePrincipalAuthenticationFactory : IAuthenticationServicePrincipalFactory
    {
        public IAccessToken Authenticate(string userName, SecureString password, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
               .Create(environment.AzureADClientId)
               .WithAuthority(environment.AzureADAuthority)
               .WithClientSecret(password.SecureStringToString())
               .WithDebugLoggingCallback(withDefaultPlatformLoggingEnabled: settings.Settings.ShowMSALDebugMessages)
               .Build();
            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            AuthenticationResult token = app.AcquireTokenForClient(scopes).ExecuteAsync().Result;
            return token.ToIAccessToken();
        }

        public IAccessToken Authenticate(string clientId, string thumbprint, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            var certificate = FindCertificate(thumbprint);
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
               .Create(environment.AzureADClientId)
               .WithAuthority(environment.AzureADAuthority)
               .WithCertificate(certificate)
               .WithDebugLoggingCallback(withDefaultPlatformLoggingEnabled: settings.Settings.ShowMSALDebugMessages)
               .Build();
            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            AuthenticationResult token = app.AcquireTokenForClient(scopes).ExecuteAsync().Result;
            return token.ToIAccessToken();
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

        public async Task Challenge(ICollection<IPowerBIEnvironment> environments)
        {
            foreach (var environment in environments)
            {
                var thumbprint = "TODO";
                var certificate = FindCertificate(thumbprint);
                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                   .Create(environment.AzureADClientId)
                   .WithAuthority(environment.AzureADAuthority)
                   .WithCertificate(certificate)
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