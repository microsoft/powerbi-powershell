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
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Abstractions.Utilities;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class WindowsAuthenticationFactory : IAuthenticationUserFactory
    {
        private static bool authenticatedOnce = false;
        private static object tokenCacheLock = new object();

        private static TokenCache Cache { get; set;}

        private static StringBuilder WindowsAuthProcessErrors = new StringBuilder();

        public bool AuthenticatedOnce { get => authenticatedOnce; }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            return HandleAuthentication(environment, logger, settings);
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            return HandleAuthentication(environment, logger, settings);
        }

        private IAccessToken HandleAuthentication(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("Authenticator only works on Windows");
            }

            if (!this.AuthenticatedOnce)
            {
                throw new AuthenticationException("Failed to authenticate once");
            }

            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            IPublicClientApplication app = PublicClientApplicationBuilder
                .Create(environment.AzureADClientId)
                .WithAuthority(environment.AzureADAuthority)
                .WithDebugLoggingCallback(withDefaultPlatformLoggingEnabled: settings.Settings.ShowMASLDebugMessages)
                .Build();
            AuthenticationResult result = null;
            var accounts = app.GetAccountsAsync().Result;
            
            try
            {
                result = app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync().Result;
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent.
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                logger.WriteDebug($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    result = app.AcquireTokenInteractive(scopes).ExecuteAsync().Result;
                }
                catch (MsalException msalex)
                {
                    throw new AuthenticationException($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                }
            }
            catch (Exception ex)
            {
                throw new AuthenticationException($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
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

        private static void WindowAuthProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            WindowsAuthProcessErrors.Append(e.Data);
        }

        private static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var fileUri = new UriBuilder(codeBase);
            var directory = Uri.UnescapeDataString(fileUri.Path);
            directory = Path.GetDirectoryName(directory);
            if (string.IsNullOrEmpty(fileUri.Host))
            {
                return directory;
            }
            else
            {
                // Running on a fileshare
                directory = $"\\\\{fileUri.Host}\\" + directory.TrimStart(new[] { '\\' });
                return directory;
            }
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