/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class DeviceCodeAuthenticationFactory : IAuthenticationUserFactory
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
            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            if (this.AuthApplication == null)
            {
                this.AuthApplication = PublicClientApplicationBuilder
                .Create(environment.AzureADClientId)
                .WithAuthority(environment.AzureADAuthority)
                .WithLogging((level, message, containsPii) => LoggingUtils.LogMsal(level, message, containsPii, logger))
                .WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows))
                .WithParentActivityOrWindow(GetConsoleOrTerminalWindow)
                .Build();
            }

            AuthenticationResult result = null;
            var accounts = await AuthApplication.GetAccountsAsync();
            if (accounts != null && accounts.Any())
            {
                try
                {
                    result = await AuthApplication.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                    return result.ToIAccessToken();
                }
                catch (MsalUiRequiredException)
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
                while (accounts != null && accounts.Any())
                {
                    await this.AuthApplication.RemoveAsync(accounts.First());
                    accounts = (await this.AuthApplication.GetAccountsAsync()).ToList();
                }

                this.AuthApplication = null;
            }
        }
    }
}
