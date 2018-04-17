/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
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

            var cloudEnvironments = GetGlobalServiceConfig().Result;

            // Ignore non-valid environments
            var environments = this.Configuration.GetSection(PowerBISettingNames.Environments.SectionName)
                .GetChildren()
                .Where(e => Enum.TryParse(e[PowerBISettingNames.Environments.Name], out PowerBIEnvironmentType result))
                .Select(e =>
                    {
                        if (!string.IsNullOrEmpty(e[PowerBISettingNames.Environments.CloudName]))
                        {
                            string cloudName = e[PowerBISettingNames.Environments.CloudName];
                            var cloudEnvironment = cloudEnvironments.Environments.FirstOrDefault(c => c.CloudName.Equals(cloudName, StringComparison.OrdinalIgnoreCase));
                            if (cloudEnvironment == null)
                            {
                                throw new NotSupportedException($"Unable to find cloud name: {cloudName}");
                            }

                            var backendService = cloudEnvironment.Services.First(s => s.Name.Equals("powerbi-backend", StringComparison.OrdinalIgnoreCase));
                            return new PowerBIEnvironment()
                            {
                                Name = (PowerBIEnvironmentType)Enum.Parse(typeof(PowerBIEnvironmentType), e[PowerBISettingNames.Environments.Name]),
                                AzureADAuthority = cloudEnvironment.Services.First(s => s.Name.Equals("aad", StringComparison.OrdinalIgnoreCase)).Endpoint,
                                AzureADClientId = e[PowerBISettingNames.Environments.ClientId],
                                AzureADRedirectAddress = e[PowerBISettingNames.Environments.Redirect],
                                AzureADResource = backendService.ResourceId,
                                GlobalServiceEndpoint = backendService.Endpoint
                            };
                        }
                        else
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
                        }
                    })
                 .Cast<IPowerBIEnvironment>().ToDictionary(e => e.Name, e => e);
            this.Environments = environments;
        }

        public async Task<GSEnvironments> GetGlobalServiceConfig(string clientName = "powerbi-msolap")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.PostAsync("https://api.powerbi.com/powerbi/globalservice/v201606/environments/discover?client=" + clientName, null);
                var serializer = new DataContractJsonSerializer(typeof(GSEnvironments));

                return serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as GSEnvironments;
            }
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