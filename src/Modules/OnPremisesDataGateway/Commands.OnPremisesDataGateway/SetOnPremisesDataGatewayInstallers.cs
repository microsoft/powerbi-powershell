using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(void))]
    public class SetOnPremisesDataGatewayInstallers : PowerBIClientCmdlet
    {
        public const string CmdletName = "OnPremisesDataGatewayInstallers";
        public const string CmdletVerb = VerbsCommon.Set;

        [Parameter(Mandatory = false)]
        public string[] PrincipalObjectIds { get; set; }

        [Parameter()]
        public OperationType Operation { get; set; }

        [Parameter()]
        public GatewayType GatewayType { get; set; }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new UpdateGatewayInstallersRequest
                {
                    Ids = PrincipalObjectIds,
                    Operation = Operation,
                    GatewayType = GatewayType
                };

                var result = client.GatewaysV2.UpdateInstallerPrincipals(request).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
