/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
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
        #endregion

        #region Parameters
        [Parameter(Mandatory = false)]
        public PowerBIEnvironmentType Environment { get; set; } = PowerBIEnvironmentType.Public;

        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = true)]
        public PSCredential Credential { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public string CertificateThumbprint { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public string ApplicationId { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = true)]
        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public SwitchParameter ServicePrincipal { get; set; }

        [Alias("TenantId")]
        [Parameter(ParameterSetName = UserParameterSet, Mandatory = false)]
        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = false)]
        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = false)]
        public string Tenant { get; set; }
        #endregion

        #region Constructors
        public ConnectPowerBIServiceAccount() : base() { }

        public ConnectPowerBIServiceAccount(IPowerBICmdletInitFactory init) : base(init) { }
        #endregion

        public override void ExecuteCmdlet()
        {
            var environment = this.Settings.Environments[this.Environment];
            if(!string.IsNullOrEmpty(this.Tenant))
            {
                var tempEnvironment = (PowerBIEnvironment) environment;
                tempEnvironment.AzureADAuthority = tempEnvironment.AzureADAuthority.ToLowerInvariant().Replace("/common/", $"/{this.Tenant}/");
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

        protected override bool CmdletManagesProfile { get => true; set => base.CmdletManagesProfile = value; }
    }
}