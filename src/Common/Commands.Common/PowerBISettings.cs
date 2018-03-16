/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    public class PowerBISettings : IPowerBISettings
    {
        private IConfigurationRoot Configuration { get; }

        public PowerBISettings(IConfigurationBuilder builder = null)
        {
            var executingDirectory = this.GetExecutingDirectory();
            string settingsFilePath = Path.Combine(executingDirectory, PowerBISettingNames.FileName);
            if(!File.Exists(settingsFilePath))
            {
                throw new FileNotFoundException("Unable to find setting configuration", settingsFilePath);
            }

            builder = builder ?? new ConfigurationBuilder().AddJsonFile(settingsFilePath);
            this.Configuration = builder.Build();

            var settings = this.Configuration.GetSection(PowerBISettingNames.SettingsSection.SectionName).GetChildren().ToDictionary(x => x.Key, x => x.Value);
            this.Settings = settings ?? throw new InvalidOperationException("Failed to load settings");

            // Ignore non-valid environments
            var environments = this.Configuration.GetSection(PowerBISettingNames.Environments.SectionName).GetChildren().Where(e => Enum.TryParse<PowerBIEnvironmentType>(e[PowerBISettingNames.Environments.Name], out PowerBIEnvironmentType result)).Select(e =>
            {
                return new PowerBIEnvironment()
                {
                    Name = (PowerBIEnvironmentType)Enum.Parse(typeof(PowerBIEnvironmentType), e[PowerBISettingNames.Environments.Name]),
                    AzureADAuthority = e[PowerBISettingNames.Environments.Authority],
                    AzureADClientId = e[PowerBISettingNames.Environments.ClientId],
                    AzureADRedirectAddress = e[PowerBISettingNames.Environments.Redirect],
                    AzureADResource = e[PowerBISettingNames.Environments.Resource],
                    GlobalServiceEndpoint = e[PowerBISettingNames.Environments.GlobalService]
                };
            }).Cast<IPowerBIEnvironment>().ToDictionary(e => e.Name, e => e);
            this.Environments = environments;
        }

        public IDictionary<PowerBIEnvironmentType, IPowerBIEnvironment> Environments { get; }

        public IDictionary<string, string> Settings { get; }

        private string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var fileUri = new UriBuilder(codeBase);
            var directory = Uri.UnescapeDataString(fileUri.Path);
            directory = Path.GetDirectoryName(directory);
            return directory;
        }
    }
}