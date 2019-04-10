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
    public class GetOnPremisesDataGatewayClusters : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayCluster";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster")]
        [Parameter(Mandatory = false, ParameterSetName = "GatewayClusterId")]
        public Guid GatewayClusterId { get; set; }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                if (this.GatewayClusterId != null)
                {
                    var gatewayCluster = client.GatewaysV2.GetGatewayClusters(this.GatewayClusterId, this.Scope == PowerBIUserScope.Individual);
                    Logger.WriteObject(gatewayCluster, true);
                }
                else
                {
                    var gatewayClusters = client.GatewaysV2.GetGatewayClusters(this.Scope == PowerBIUserScope.Individual);
                    Logger.WriteObject(gatewayClusters, true);
                }
            }
        }
    }
}
