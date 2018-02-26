using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Profile
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = UserParameterSet)]
    [Alias("Login-PowerBIServiceAccount", "Set-PowerBIServiceAccount", "Add-PowerBIServiceAccount")]
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
        public PowerBIEnvironmentType Environment { get; set; }

        [Parameter(ParameterSetName = UserParameterSet, Mandatory = false)]
        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = true)]
        public PSCredential Credential { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public string CertificateThumbprint { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public string ApplicationId { get; set; }

        [Parameter(ParameterSetName = ServicePrincipalParameterSet, Mandatory = true)]
        [Parameter(ParameterSetName = ServicePrincipalCertificateParameterSet, Mandatory = true)]
        public SwitchParameter ServicePrincipal { get; set; }
        #endregion

        #region Constructors
        public ConnectPowerBIServiceAccount() : base() { }

        public ConnectPowerBIServiceAccount(IPowerBICmdletInitFactory init) : base(init) { }
        #endregion

        protected override void ExecuteCmdlet()
        {
            var environment = this.Settings.Environments[this.Environment];

            this.Authenticator.Challenge(); // revoke any previous login
            var profileType = PowerBIProfileType.User;
            IAccessToken token = null;
            switch (this.ParameterSetName)
            {
                case UserParameterSet:
                    profileType = PowerBIProfileType.User;
                    token = this.Authenticator.Authenticate(environment, this.Logger, this.Settings, new Dictionary<string, string>()
                        {
                            { "prompt", "select_account" },
                            { "msafed", "0" }
                        }
                    );
                    break;
                default:
                    throw new NotImplementedException($"Parameter set {this.ParameterSetName} was not implemented");
            }

            var profile = new PowerBIProfile(environment, token, profileType);
            this.Storage.SetItem("profile", profile);
            this.Logger.WriteObject(profile);
        }

        protected override bool CmdletManagesProfile { get => true; set => base.CmdletManagesProfile = value; }
    }
}