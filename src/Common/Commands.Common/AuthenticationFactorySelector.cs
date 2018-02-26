using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Authentication;

namespace Microsoft.PowerBI.Commands.Common
{
    public class AuthenticationFactorySelector : IAuthenticationFactory
    {
        private static IAuthenticationFactory InitAuthFactory;

        public bool AuthenticatedOnce => InitAuthFactory != null && InitAuthFactory.AuthenticatedOnce;

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            if (InitAuthFactory == null)
            {
                bool forceDeviceAuth = false;
                if(settings.Settings.TryGetValue(PowerBISettingNames.SettingsSection.ForceDeviceCodeAuthentication, out string forceDeviceAuthString) 
                    && bool.TryParse(forceDeviceAuthString, out bool forceDeviceAuthParsed))
                {
                    forceDeviceAuth = forceDeviceAuthParsed;
                }


                if (!forceDeviceAuth && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    InitAuthFactory = new WindowsAuthenticationFactory();
                }
                else
                {
                    InitAuthFactory = new DeviceCodeAuthenticationFactory();
                }
            }

            return InitAuthFactory.Authenticate(environment, logger, settings, queryParameters);
        }

        public void Challenge()
        {
            InitAuthFactory?.Challenge();
        }
    }
}