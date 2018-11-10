/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication;
using System.Text;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

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
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("Authenticator only works on Windows");
            }

            LoggerCallbackHandler.UseDefaultLogging = settings.Settings.ShowADALDebugMessages;

            string queryParamString = queryParameters.ToQueryParameterString();
            if(Cache == null)
            {
                lock(tokenCacheLock)
                {
                    if(Cache == null)
                    {
                        Cache = InitializeCache(environment, queryParamString);
                    }
                }
            }

            if(!this.AuthenticatedOnce)
            {
                throw new AuthenticationException("Failed to authenticate once");
            }

            var context = new AuthenticationContext(environment.AzureADAuthority, Cache);
            AuthenticationResult token = null;
            try
            {
                token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            }
            catch(AdalSilentTokenAcquisitionException)
            {
                // ignore and try one more time by getting a new cache and let the exception fly if it fails
                lock (tokenCacheLock)
                {
                    Cache = InitializeCache(environment, queryParamString);
                }

                context = new AuthenticationContext(environment.AzureADAuthority, Cache);
                token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            }

            return token.ToIAccessToken();
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("Authenticator only works on Windows");
            }

            LoggerCallbackHandler.UseDefaultLogging = settings.Settings.ShowADALDebugMessages;

            if (Cache == null)
            {
                lock (tokenCacheLock)
                {
                    if (Cache == null)
                    {
                        Cache = InitializeCache(environment, null, userName, password.SecureStringToString());
                    }
                }
            }

            if (!this.AuthenticatedOnce)
            {
                throw new AuthenticationException("Failed to authenticate once");
            }

            var context = new AuthenticationContext(environment.AzureADAuthority, Cache);
            AuthenticationResult token = null;
            try
            {
                token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                // ignore and try one more time by getting a new cache and let the exception fly if it fails
                lock (tokenCacheLock)
                {
                    Cache = InitializeCache(environment, null, userName, password.SecureStringToString());
                }

                context = new AuthenticationContext(environment.AzureADAuthority, Cache);
                token = context.AcquireTokenSilentAsync(environment.AzureADResource, environment.AzureADClientId).Result;
            }

            return token.ToIAccessToken();
        }

        private static TokenCache InitializeCache(IPowerBIEnvironment environment, string queryParams, string userName = null, string password = null)
        {
            using (var windowAuthProcess = new Process())
            {
                var executingDirectory = GetExecutingDirectory();

                windowAuthProcess.StartInfo.FileName = Path.Combine(executingDirectory, "WindowsAuthenticator", "AzureADWindowsAuthenticator.exe");
                windowAuthProcess.StartInfo.Arguments = $"-Authority:\"{environment.AzureADAuthority}\" -Resource:\"{environment.AzureADResource}\" -ID:\"{environment.AzureADClientId}\" -Redirect:\"{environment.AzureADRedirectAddress}\" -Query:\"{queryParams}\"";
                if(userName != null && password != null)
                {
                    var pwBytes = Encoding.UTF8.GetBytes(password);
                    var pwBase64 = Convert.ToBase64String(pwBytes);
                    // TODO encrypt with AES or MachineKey (as long as it works with .NET Framework and .NET Core)
                    windowAuthProcess.StartInfo.Arguments += $" -User:\"{userName}\" -PW:\"{pwBase64}\"";
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
            return directory;
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