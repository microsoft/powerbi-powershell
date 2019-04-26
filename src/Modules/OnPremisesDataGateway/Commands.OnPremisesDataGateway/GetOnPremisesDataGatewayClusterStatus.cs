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
    [OutputType(typeof(IEnumerable<GatewayCluster>))]
    public class GetOnPremisesDataGatewayClusterStatus : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterStatus";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster")]
        [Parameter()]
        public Guid GatewayClusterId { get; set; }

        public GetOnPremisesDataGatewayClusterStatus() : base() { }

        public GetOnPremisesDataGatewayClusterStatus(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var gatewayCluster = client.Gateways.GetGatewayClusterStatus(this.GatewayClusterId, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(gatewayCluster, true);
            }
        }
    }
}
