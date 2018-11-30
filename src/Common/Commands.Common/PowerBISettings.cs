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
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Abstractions.Utilities;

namespace Microsoft.PowerBI.Commands.Common
{
    public class PowerBISettings : IPowerBISettings
    {
        public const string FileName = "settings.json";

        private static GSEnvironments GlobalServiceEnvironments { get; set; }

        public PowerBISettings(string settingsFilePath = null)
        {
            if(string.IsNullOrEmpty(settingsFilePath))
            {
                var executingDirectory = DirectoryUtility.GetExecutingDirectory();
                settingsFilePath = Path.Combine(executingDirectory, FileName);
            }
            
            if(!File.Exists(settingsFilePath))
            {
                throw new FileNotFoundException("Unable to find setting configuration", settingsFilePath);
            }

            PowerBIConfiguration configuration = null;
            using (var fileReader = File.OpenRead(settingsFilePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(PowerBIConfiguration));
                using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(fileReader, Encoding.UTF8, XmlDictionaryReaderQuotas.Max, null))
                {
                    configuration = serializer.ReadObject(jsonReader) as PowerBIConfiguration;
                }
            }

            this.Settings = configuration.Settings;
            var cloudEnvironments = GetGlobalServiceConfig().Result;

            // Ignore non-valid environments
            var environments = configuration.Environments
                .Where(e => Enum.TryParse(e.Name, out PowerBIEnvironmentType result))
                .Select(e =>
                    {
                        if (!string.IsNullOrEmpty(e.CloudName))
                        {
                            string cloudName = e.CloudName;
                            var cloudEnvironment = cloudEnvironments.Environments.FirstOrDefault(c => c.CloudName.Equals(cloudName, StringComparison.OrdinalIgnoreCase));
                            if (cloudEnvironment == null)
                            {
                                throw new NotSupportedException($"Unable to find cloud name: {cloudName}");
                            }

                            var backendService = cloudEnvironment.Services.First(s => s.Name.Equals("powerbi-backend", StringComparison.OrdinalIgnoreCase));
                            return new PowerBIEnvironment()
                            {
                                Name = (PowerBIEnvironmentType)Enum.Parse(typeof(PowerBIEnvironmentType), e.Name),
                                AzureADAuthority = cloudEnvironment.Services.First(s => s.Name.Equals("aad", StringComparison.OrdinalIgnoreCase)).Endpoint,
                                AzureADClientId = e.ClientId,
                                AzureADRedirectAddress = e.Redirect,
                                AzureADResource = backendService.ResourceId,
                                GlobalServiceEndpoint = backendService.Endpoint
                            };
                        }
                        else
                        {
                            return new PowerBIEnvironment()
                            {
                                Name = (PowerBIEnvironmentType)Enum.Parse(typeof(PowerBIEnvironmentType), e.Name),
                                AzureADAuthority = e.Authority,
                                AzureADClientId = e.ClientId,
                                AzureADRedirectAddress = e.Redirect,
                                AzureADResource = e.Resource,
                                GlobalServiceEndpoint = e.GlobalService
                            };
                        }
                    })
                 .Cast<IPowerBIEnvironment>().ToDictionary(e => e.Name, e => e);
            this.Environments = environments;
        }

        public async Task<GSEnvironments> GetGlobalServiceConfig(string clientName = "powerbi-msolap")
        {
            if(GlobalServiceEnvironments == null)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.PostAsync("https://api.powerbi.com/powerbi/globalservice/v201606/environments/discover?client=" + clientName, null);
                    var serializer = new DataContractJsonSerializer(typeof(GSEnvironments));

                    GlobalServiceEnvironments = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as GSEnvironments;
                }
            }

            return GlobalServiceEnvironments;
        }

        public IDictionary<PowerBIEnvironmentType, IPowerBIEnvironment> Environments { get; }

        public IPowerBIConfigurationSettings Settings { get; }
    }
}