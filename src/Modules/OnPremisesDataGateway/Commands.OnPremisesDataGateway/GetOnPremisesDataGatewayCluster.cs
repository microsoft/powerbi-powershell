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
    public class GetOnPremisesDataGatewayCluster : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayCluster";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var gatewayClusters = client.GatewaysV2.GetGatewayClusters(this.Scope == PowerBIUserScope.Individual);
                Logger.WriteObject(gatewayClusters, true);
            }
        }
    }
}
