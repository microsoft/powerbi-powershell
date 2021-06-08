/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Profile
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = UserParameterSet)]
    [Alias("Login-PowerBIServiceAccount", "Set-PowerBIServiceAccount", "Add-PowerBIServiceAccount", "Login-PowerBI")]
    [OutputType(typeof(IPowerBIProfile))]
    public class ConnectPowerBIServiceAccount : PowerBICmdlet
    {
        #region Cmdlet name
        public const string CmdletName = "PowerBIServiceAccount";
        public const string CmdletVerb = VerbsCommunications.Connect;
        #endregion

        #region Parameter set names
        public const string UserParameterSet = "User";
        public const string ServicePrincipalParameterSet = "ServicePrincipal";
        public const string ServicePrincipalCertificateParameterSet = "ServicePrincipalCertificate";
        public const string UserAndCredentialPasswordParameterSet = "UserAndCredential";
        #endregion

        #region Parameters
        [Parameter(Mandatory = false)]
        public PowerBIEnvironmentType Environment { get; set; } = PowerBIEnvironmentType.Public;

        [Parameter(Mandatory = false)]
        public string CustomEnvironment { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = true)]
        [Parameter(ParameterSetName = UserAndCredentialPasswordParameterSet, Mandatory = true)]
        public PSCredential Credential { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public string CertificateThumbprint { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public string ApplicationId { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = true)]
        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public SwitchParameter ServicePrincipal { get; set; }

        [Alias("TenantId")]
        [Parameter(Mandatory = false)]
        public string Tenant { get; set; }

        [Parameter(Mandatory = false)]
        public string DiscoveryUrl { get; set; }
        #endregion

        /// <summary>
        /// Custom environment settings to pick from.
        /// </summary>
        private IDictionary<string, IPowerBIEnvironment> CustomEnvironments { get; set; }

        private GSEnvironments CustomServiceEnvironments { get; set; }

        #region Constructors
        public ConnectPowerBIServiceAccount() : base() { }

        public ConnectPowerBIServiceAccount(IPowerBICmdletInitFactory init) : base(init) { }
        #endregion

        public override void ExecuteCmdlet()
        {
            IPowerBIEnvironment environment = null;

            // Populate custom environments from discovery url if it is present
            // otherwise get environment from existing settings
            if (!string.IsNullOrEmpty(this.DiscoveryUrl))
            {
                if (string.IsNullOrEmpty(this.CustomEnvironment))
                {
                    throw new Exception($"{nameof(this.CustomEnvironment)} is required when using a discovery url");
                }

                var settings = new PowerBISettings();

                CustomEnvironments = new Dictionary<string, IPowerBIEnvironment>();
                var customCloudEnvironments = GetServiceConfig(this.DiscoveryUrl).Result;
                foreach (GSEnvironment customEnvironment in customCloudEnvironments.Environments)
                {
                    var backendService = customEnvironment.Services.First(s => s.Name.Equals("powerbi-backend", StringComparison.OrdinalIgnoreCase));
                    var redirectApp = settings.Environments[PowerBIEnvironmentType.Public];
                    var env = new PowerBIEnvironment()
                    {
                        Name = PowerBIEnvironmentType.Custom,
                        AzureADAuthority = customEnvironment.Services.First(s => s.Name.Equals("aad", StringComparison.OrdinalIgnoreCase)).Endpoint,
                        AzureADClientId = redirectApp.AzureADClientId,
                        AzureADRedirectAddress = redirectApp.AzureADRedirectAddress,
                        AzureADResource = backendService.ResourceId,
                        GlobalServiceEndpoint = backendService.Endpoint
                    };

                    this.CustomEnvironments.Add(customEnvironment.CloudName, env);
                }

                if (!this.CustomEnvironments.ContainsKey(this.CustomEnvironment))
                {
                    this.Logger.ThrowTerminatingError($"Discovery URL {this.DiscoveryUrl} did not return environment {this.CustomEnvironment}");
                }
                environment = this.CustomEnvironments[this.CustomEnvironment];
            }
            else
            {
                var settings = new PowerBISettings(targetEnvironmentType: this.Environment, refreshGlobalServiceConfig: true);
                if (settings.Environments == null)
                {
                    this.Logger.ThrowTerminatingError("Failed to populate environments in settings");
                }
                environment = settings.Environments[this.Environment];
            }

            if(!string.IsNullOrEmpty(this.Tenant))
            {
                var tempEnvironment = (PowerBIEnvironment) environment;
                tempEnvironment.AzureADAuthority = tempEnvironment.AzureADAuthority.ToLowerInvariant().Replace("/common", $"/{this.Tenant}");
                this.Logger.WriteVerbose($"Updated Azure AD authority with -Tenant specified, new value: {tempEnvironment.AzureADAuthority}");
                environment = tempEnvironment;
            }

            this.Authenticator.Challenge(); // revoke any previous login
            IAccessToken token = null;
            PowerBIProfile profile = null;
            switch (this.ParameterSet)
            {
                case UserParameterSet:
                    token = this.Authenticator.Authenticate(environment, this.Logger, this.Settings, new Dictionary<string, string>()
                        {
                            { "prompt", "select_account" },
                            { "msafed", "0" }
                        }
                    );
                    profile = new PowerBIProfile(environment, token);
                    break;
                case UserAndCredentialPasswordParameterSet:
                    token = this.Authenticator.Authenticate(environment, this.Logger, this.Settings, this.Credential.UserName, this.Credential.Password);
                    profile = new PowerBIProfile(environment, this.Credential.UserName, this.Credential.Password, token, servicePrincipal: false);
                    break;
                case ServicePrincipalCertificateParameterSet:
                    token = this.Authenticator.Authenticate(this.ApplicationId, this.CertificateThumbprint, environment, this.Logger, this.Settings);
                    profile = new PowerBIProfile(environment, this.ApplicationId, this.CertificateThumbprint, token);
                    break;
                case ServicePrincipalParameterSet:
                    token = this.Authenticator.Authenticate(this.Credential.UserName, this.Credential.Password, environment, this.Logger, this.Settings);
                    profile = new PowerBIProfile(environment, this.Credential.UserName, this.Credential.Password, token);
                    break;
                default:
                    throw new NotImplementedException($"Parameter set {this.ParameterSet} was not implemented");
            }

            this.Storage.SetItem("profile", profile);
            this.Logger.WriteObject(profile);
        }

        private async Task<GSEnvironments> GetServiceConfig(string discoveryUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.PostAsync(discoveryUrl, null);
                var serializer = new DataContractJsonSerializer(typeof(GSEnvironments));

                this.CustomServiceEnvironments = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as GSEnvironments;
            }

            return this.CustomServiceEnvironments;
        } 

        protected override bool CmdletManagesProfile { get => true; set => base.CmdletManagesProfile = value; }
    }
}