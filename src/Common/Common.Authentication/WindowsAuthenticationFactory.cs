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

            IEnumerable<string> scopes = new[] { $"{environment.AzureADResource}/.default" };
            IPublicClientApplication app = PublicClientApplicationBuilder
                .Create(environment.AzureADClientId)
                .WithAuthority(environment.AzureADAuthority)
                .WithDebugLoggingCallback(withDefaultPlatformLoggingEnabled: settings.Settings.ShowMSALDebugMessages)
                .Build();
            AuthenticationResult result = null;
            var accounts = app.GetAccountsAsync().Result;

            try
            {
                if (accounts.Any())
                {
                    result = app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync().Result;
                }
                else
                {
                    // This indicates you need to call AcquireTokenInteractive to acquire a token
                    result = app.AcquireTokenInteractive(scopes).ExecuteAsync().Result;
                }
            }
            catch (MsalException msalex)
            {
                throw new AuthenticationException($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
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
        /*
        private static void InitializeTokenCache(IPowerBIEnvironment environment, string queryParams, string userName = null, SecureString password = null)
        {
            using (var windowAuthProcess = new Process())
            {
                var executingDirectory = DirectoryUtility.GetExecutingDirectory();

                windowAuthProcess.StartInfo.FileName = Path.Combine(executingDirectory, "WindowsAuthenticator", "AzureADWindowsAuthenticator.exe");
                windowAuthProcess.StartInfo.Arguments = $"-Authority:\"{environment.AzureADAuthority}\" -Resource:\"{environment.AzureADResource}\" -ID:\"{environment.AzureADClientId}\" -Redirect:\"{environment.AzureADRedirectAddress}\" -Query:\"{queryParams}\"";
                if(userName != null && password != null)
                {
                    var pwBytes = Encoding.UTF8.GetBytes(password.SecureStringToString());
                    var pwBase64 = Convert.ToBase64String(pwBytes);
                    // TODO encrypt with AES or MachineKey (as long as it works with .NET Framework and .NET Core)
                    windowAuthProcess.StartInfo.Arguments += $" -User:\"{userName}\" -PW:\"{password}\"";
                }

                windowAuthProcess.StartInfo.UseShellExecute = false;
                windowAuthProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                windowAuthProcess.StartInfo.RedirectStandardOutput = true;
                windowAuthProcess.StartInfo.RedirectStandardError = true;

                windowAuthProcess.ErrorDataReceived += WindowAuthProcess_ErrorDataReceived;

                windowAuthProcess.Start();
                windowAuthProcess.BeginErrorReadLine();

                var result = windowAuthProcess.StandardOutput.ReadToEnd();
                windowAuthProcess.WaitForExit();

                if (windowAuthProcess.ExitCode != 0)
                {
                    string errorMessage = WindowsAuthProcessErrors.ToString();
                    WindowsAuthProcessErrors.Clear();
                    throw new AdalException("0", $"Failed to get ADAL token: {errorMessage}");
                }

                authenticatedOnce = true;
                var tokeCacheBytes = Convert.FromBase64String(result);
                return new TokenCache(tokeCacheBytes);
            }
        }
        */
        public void Challenge()
        {
            authenticatedOnce = false;
        }
    }
}