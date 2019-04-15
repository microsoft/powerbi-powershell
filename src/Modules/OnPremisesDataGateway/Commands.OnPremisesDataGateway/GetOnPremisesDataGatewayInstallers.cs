using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<InstallerPrincipal>))]
    public class GetOnPremisesDataGatewayInstallers : PowerBIClientCmdlet
    {
        public const string CmdletName = "OnPremisesDataGatewayInstallers";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter(Mandatory = false)]
        public GatewayType? GatewayTypeParameter { get; set; } = null;

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var gatewayInstallerPrincipals = client.GatewaysV2.GetInstallerPrincipals(GatewayTypeParameter).Result;
                Logger.WriteObject(gatewayInstallerPrincipals, true);
            }
        }
    }
}
