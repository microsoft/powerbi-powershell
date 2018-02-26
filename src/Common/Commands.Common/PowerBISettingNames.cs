namespace Microsoft.PowerBI.Commands.Common
{
    public static class PowerBISettingNames
    {
        public const string FileName = "settings.json";

        public static class SettingsSection
        {
            public const string SectionName = "Settings";

            public const string ForceDeviceCodeAuthentication = "ForceDeviceCodeAuthentication";
        }

        public static class Environments
        {
            public const string SectionName = "Environments";

            public const string Name = "name";
            public const string Authority = "authority";
            public const string ClientId = "clientId";
            public const string Redirect = "redirect";
            public const string Resource = "resource";
            public const string Override = "override";
            public const string GlobalService = "globalService";
        }
    }
}