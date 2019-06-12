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
    [OutputType(typeof(GatewayClusterDatasource))]
    public class AddOnPremisesDataGatewayClusterDatasource : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterDatasource";
        public const string CmdletVerb = VerbsCommon.Add;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Parameter(Mandatory = true)]
        public string ConnectionDetails { get; set; }

        [Parameter(Mandatory = true)]
        public IDictionary<Guid, DatasourceCredentialDetails> CredentialDetailsDictionary { get; set; }

        [Parameter(Mandatory = true)]
        public string DatasourceName { get; set; }

        [Parameter(Mandatory = true)]
        public string DatasourceType { get; set; }

        [Parameter()]
        public string Annotation { get; set; }

        [Parameter()]
        public string SingleSignOnType { get; set; }

        public AddOnPremisesDataGatewayClusterDatasource() : base() { }

        public AddOnPremisesDataGatewayClusterDatasource(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new PublishDatasourceToGatewayClusterRequest
                {
                    Annotation = Annotation,
                    ConnectionDetails = ConnectionDetails,
                    CredentialDetailsDictionary = CredentialDetailsDictionary,
                    DatasourceName = DatasourceName,
                    DatasourceType = DatasourceType,
                    SingleSignOnType = SingleSignOnType,
                };

                var result = client.Gateways.CreateGatewayClusterDatasource(GatewayClusterId, request, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
