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
    [OutputType(typeof(GatewayTenant))]
    public class GetOnPremisesDataGatewayTenantPolicy : PowerBIClientCmdlet
    {
        public const string CmdletName = "OnPremisesDataGatewayTenantPolicy";
        public const string CmdletVerb = VerbsCommon.Get;

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var gatewayTenantPolicy = client.Gateways.GetTenantPolicy().Result;
                Logger.WriteObject(gatewayTenantPolicy, true);
            }
        }
    }
}
