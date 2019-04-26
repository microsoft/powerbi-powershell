/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(void))]
    public class SetOnPremisesDataGatewayInstaller : PowerBIClientCmdlet
    {
        public const string CmdletName = "OnPremisesDataGatewayInstallers";
        public const string CmdletVerb = VerbsCommon.Set;

        [Alias("Users")]
        [Parameter()]
        public string[] PrincipalObjectIds { get; set; }

        [Parameter(Mandatory = true)]
        public OperationType Operation { get; set; }

        [Parameter(Mandatory = true)]
        public GatewayType GatewayType { get; set; }

        public SetOnPremisesDataGatewayInstaller() : base() { }

        public SetOnPremisesDataGatewayInstaller(IPowerBIClientCmdletInitFactory init) : base(init) { }

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

                var result = client.Gateways.UpdateInstallerPrincipals(request).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
