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
    public class SetOnPremisesDataGatewayTenantPolicy : PowerBIClientCmdlet
    {
        public const string CmdletName = "OnPremisesDataGatewayTenantPolicy";
        public const string CmdletVerb = VerbsCommon.Set;

        [Parameter()]
        public PolicyType ResourceGatewayInstallPolicy { get; set; }

        [Parameter()]
        public PolicyType PersonalGatewayInstallPolicy { get; set; }

        public SetOnPremisesDataGatewayTenantPolicy() : base() { }

        public SetOnPremisesDataGatewayTenantPolicy(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new UpdateGatewayPolicyRequest
                {
                    ResourceGatewayInstallPolicy = ResourceGatewayInstallPolicy,
                    PersonalGatewayInstallPolicy = PersonalGatewayInstallPolicy
                };

                var result = client.Gateways.UpdateTenantPolicy(request).Result;
                Logger.WriteDebug(result);
            }
        }
    }
}
