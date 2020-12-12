/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Authentication;

namespace Microsoft.PowerBI.Commands.Common
{
    public class AuthenticationFactorySelector : IAuthenticationFactory
    {
        private static IAuthenticationUserFactory UserAuthFactory;
        private static IAuthenticationServicePrincipalFactory ServicePrincipalAuthFactory;
        private static IAuthenticationBaseFactory BaseAuthFactory;

        private object authFactoryLock = new object();
        
        private void InitializeUserAuthenticationFactory(IPowerBILogger logger, IPowerBISettings settings)
        {
            if (UserAuthFactory == null)
            {
                lock (this.authFactoryLock)
                {
                    if (UserAuthFactory == null)
                    {
                        bool forceDeviceAuth = settings.Settings.ForceDeviceCodeAuthentication;
                        if (!forceDeviceAuth && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            UserAuthFactory = new WindowsAuthenticationFactory();
                        }
                        else
                        {
                            UserAuthFactory = new DeviceCodeAuthenticationFactory();
                        }
                    }
                }
            }

            BaseAuthFactory = UserAuthFactory;
        }

        private void InitializeServicePrincpalAuthenticationFactory(IPowerBILogger logger, IPowerBISettings settings)
        {
            if (ServicePrincipalAuthFactory == null)
            {
                lock (this.authFactoryLock)
                {
                    if(ServicePrincipalAuthFactory == null)
                    {
                        ServicePrincipalAuthFactory = new ServicePrincipalAuthenticationFactory();
                    }
                }
            }

            BaseAuthFactory = ServicePrincipalAuthFactory;
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            this.InitializeUserAuthenticationFactory(logger, settings);
            return UserAuthFactory.Authenticate(environment, logger, settings, queryParameters);
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            this.InitializeUserAuthenticationFactory(logger, settings);
            return UserAuthFactory.Authenticate(environment, logger, settings, userName, password);
        }

        public IAccessToken Authenticate(IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            switch (profile.LoginType)
            {
                case PowerBIProfileType.User:
                    return this.Authenticate(profile.Environment, logger, settings, queryParameters);
                case PowerBIProfileType.UserAndPassword:
                    return this.Authenticate(profile.Environment, logger, settings, profile.UserName, profile.Password);
                case PowerBIProfileType.ServicePrincipal:
                    return this.Authenticate(profile.UserName, profile.Password, profile.Environment, logger, settings);
                case PowerBIProfileType.Certificate:
                    return this.Authenticate(profile.UserName, profile.Thumbprint, profile.Environment, logger, settings);
                default:
                    throw new NotSupportedException();
            }
        }

        public async Task Challenge(ICollection<IPowerBIEnvironment> environments)
        {
            await UserAuthFactory?.Challenge(environments);
            await ServicePrincipalAuthFactory?.Challenge(environments);
        }

        public IAccessToken Authenticate(string userName, SecureString password, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            this.InitializeServicePrincpalAuthenticationFactory(logger, settings);
            return ServicePrincipalAuthFactory.Authenticate(userName, password, environment, logger, settings);
        }

        public IAccessToken Authenticate(string clientId, string thumbprint, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            this.InitializeServicePrincpalAuthenticationFactory(logger, settings);
            return ServicePrincipalAuthFactory.Authenticate(clientId, thumbprint, environment, logger, settings);
        }
    }
}