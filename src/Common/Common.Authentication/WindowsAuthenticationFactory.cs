﻿/*
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
using Microsoft.Identity.Client.Broker;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Abstractions.Utilities;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class WindowsAuthenticationFactory : IAuthenticationUserFactory
    {
        private IPublicClientApplication AuthApplication;

        private enum GetAncestorFlags
        {
            GetParent = 1,
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
            /// </summary>
            GetRootOwner = 3
        }

        /// <summary>
        /// Retrieves the handle to the ancestor of the specified window.
        /// </summary>
        /// <param name="hwnd">A handle to the window whose ancestor is to be retrieved.
        /// If this parameter is the desktop window, the function returns NULL. </param>
        /// <param name="flags">The ancestor to be retrieved.</param>
        /// <returns>The return value is the handle to the ancestor window.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        // This is your window handle!
        private IntPtr GetConsoleOrTerminalWindow()
        {
            IntPtr consoleHandle = GetConsoleWindow();
            IntPtr handle = GetAncestor(consoleHandle, GetAncestorFlags.GetRootOwner);

            return handle;
        }

        public async Task<IAccessToken> Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            return await HandleAuthentication(environment, logger, settings, queryParameters);
        }

        public async Task<IAccessToken> Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            return await HandleAuthentication(environment, logger, settings, null, userName, password);
        }

        private async Task<IAccessToken> HandleAuthentication(
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
            BuildAuthApplication(environment, queryParameters, logger);
            AuthenticationResult result = null;

            try
            {
                var accounts = await this.AuthApplication.GetAccountsAsync();
                if (accounts != null && accounts.Any())
                {
                    // This indicates there's token in cache
                    result = await this.AuthApplication.AcquireTokenSilent(scopes, accounts.First()).ExecuteAsync();
                }
                else
                {
                    // auth application is auto cleared when there's no account
                    BuildAuthApplication(environment, queryParameters, logger);
                    if (!string.IsNullOrEmpty(userName) && password != null && password.Length > 0)
                    {
                        result = await this.AuthApplication.AcquireTokenByUsernamePassword(scopes, userName, password).ExecuteAsync();
                    }
                    else
                    {
                        result = await this.AuthApplication.AcquireTokenInteractive(scopes).ExecuteAsync();
                    }
                }
            }
            catch (MsalUiRequiredException)
            {
                result = await this.AuthApplication.AcquireTokenInteractive(scopes).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new AuthenticationException($"Error Acquiring Token:{System.Environment.NewLine}{ex.Message}");
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

        public async Task Challenge()
        {
            if (this.AuthApplication != null)
            {
                var accounts = (await this.AuthApplication.GetAccountsAsync()).ToList();
                while (accounts != null && accounts.Any())
                {
                    await this.AuthApplication.RemoveAsync(accounts.First());
                    accounts = (await this.AuthApplication.GetAccountsAsync()).ToList();
                }

                this.AuthApplication = null;
            }
        }

        private void BuildAuthApplication(IPowerBIEnvironment environment, IDictionary<string, string> queryParameters, IPowerBILogger logger)
        {
            // auth application is auto cleared when there's no account
            if (this.AuthApplication == null)
            {
                var authApplicationBuilder = PublicClientApplicationBuilder
                    .Create(environment.AzureADClientId)
                    .WithAuthority(environment.AzureADAuthority)
                    .WithLogging((level, message, containsPii) => LoggingUtils.LogMsal(level, message, containsPii, logger))
                    .WithExtraQueryParameters(queryParameters)
                    .WithParentActivityOrWindow(GetConsoleOrTerminalWindow)
                    .WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows));

                if (!PublicClientHelper.IsNetFramework)
                {
                    authApplicationBuilder.WithRedirectUri("http://localhost");
                }

                this.AuthApplication = authApplicationBuilder.Build();
            }
        }
    }
}