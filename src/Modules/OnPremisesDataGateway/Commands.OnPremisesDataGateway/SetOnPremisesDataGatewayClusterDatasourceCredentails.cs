/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(DatasourceErrorDetails))]
    public class SetOnPremisesDataGatewayClusterDatasourceCredentials : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterDatasourceCredentials";
        public const string CmdletVerb = VerbsCommon.Set;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Alias("DatasourceId")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterDatasourceId { get; set; }

        [Parameter(Mandatory = true)]
        public string Credentials { get; set; }

        [Parameter(Mandatory = true)]
        public string CredentialType { get; set; }

        [Parameter(Mandatory = true)]
        public string EncryptedConnection { get; set; }

        [Parameter(Mandatory = true)]
        public string EncryptionAlgorithm { get; set; }

        [Parameter(Mandatory = true)]
        public string PrivacyLevel { get; set; }

        [Parameter()]
        public bool? SkipTestConnection { get; set; }

        [Parameter()]
        public bool? UseCallerAADIdentity { get; set; }

        [Parameter()]
        public bool? UseCustomOAuthApp { get; set; }

        [Parameter()]
        public bool? UseEndUserOAuth2Credentials { get; set; }

        public SetOnPremisesDataGatewayClusterDatasourceCredentials() : base() { }

        public SetOnPremisesDataGatewayClusterDatasourceCredentials(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new DatasourceCredentialDetails
                {
                    Credentials = Credentials,
                    CredentialType = CredentialType,
                    EncryptedConnection = EncryptedConnection,
                    EncryptionAlgorithm = EncryptionAlgorithm,
                    PrivacyLevel = PrivacyLevel,
                    SkipTestConnection = SkipTestConnection,
                    UseCallerAADIdentity = UseCallerAADIdentity,
                    UseCustomOAuthApp = UseCustomOAuthApp,
                    UseEndUserOAuth2Credentials = UseEndUserOAuth2Credentials,
                };

                var result = client.Gateways.UpdateGatewayClusterDatasourceCredentials(GatewayClusterId, GatewayClusterDatasourceId, request, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
