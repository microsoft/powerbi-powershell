/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class ServicePrincipalAuthenticationFactory : IAuthenticationServicePrincipalFactory
    {
        private static bool authenticatedOnce = false;
        public bool AuthenticatedOnce { get => authenticatedOnce; }

        private static object tokenCacheLock = new object();

        private static TokenCache Cache { get; set; }

        private AuthenticationContext InitializeContext(IPowerBIEnvironment environment, IPowerBISettings settings)
        {
            LoggerCallbackHandler.UseDefaultLogging = settings.ShowADALDebugMessages();

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

            return new AuthenticationContext(environment.AzureADAuthority, Cache);
        }

        public IAccessToken Authenticate(string userName, SecureString password, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            var context = InitializeContext(environment, settings);
            AuthenticationResult token = context.AcquireTokenAsync(environment.AzureADResource, new ClientCredential(userName, password.SecureStringToString())).Result;
            return token.ToIAccessToken();
        }

        public IAccessToken Authenticate(string clientId, string thumbprint , IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            var certificate = FindCertificate(thumbprint);
            var context = InitializeContext(environment, settings);
            AuthenticationResult token = context.AcquireTokenAsync(environment.AzureADResource, new ClientAssertionCertificate(clientId, certificate)).Result;
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
            using(var store = new X509Store(StoreName.My, location))
            {
                store.Open(OpenFlags.ReadOnly);
                certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            }

            return certificates.Count > 0;
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
