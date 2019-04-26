/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(GatewayCluster))]
    public class GetOnPremisesDataGatewayCluster : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayCluster";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster")]
        [Parameter()]
        public Guid GatewayClusterId { get; set; }

        public GetOnPremisesDataGatewayCluster() : base() { }

        public GetOnPremisesDataGatewayCluster(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                if (this.GatewayClusterId != default)
                {
                    var gatewayCluster = client.Gateways.GetGatewayClusters(this.GatewayClusterId, this.Scope == PowerBIUserScope.Individual).Result;
                    Logger.WriteObject(gatewayCluster, true);
                }
                else
                {
                    var gatewayClusters = client.Gateways.GetGatewayClusters(this.Scope == PowerBIUserScope.Individual).Result;
                    Logger.WriteObject(gatewayClusters, true);
                }
            }
        }
    }
}
