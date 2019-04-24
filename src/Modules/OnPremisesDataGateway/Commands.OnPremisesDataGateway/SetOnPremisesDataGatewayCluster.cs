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
    [OutputType(typeof(void))]
    public class SetOnPremisesDataGatewayCluster : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayCluster";
        public const string CmdletVerb = VerbsCommon.Set;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Parameter()]
        public string Name { get; set; }

        [Parameter()]
        public string Department { get; set; }

        [Parameter()]
        public string Description { get; set; }

        [Parameter()]
        public string ContactInformation { get; set; }

        [Parameter()]
        public bool? AllowCloudDatasourceRefresh { get; set; }

        [Parameter()]
        public bool? AllowCustomConnectors { get; set; }

        [Parameter()]
        public string LoadBalancingSelectorType { get; set; }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new PatchGatewayClusterRequest
                {
                    Name = Name,
                    Department = Department,
                    Description = Description,
                    ContactInformation = ContactInformation,
                    AllowCloudDatasourceRefresh = AllowCloudDatasourceRefresh,
                    AllowCustomConnectors = AllowCustomConnectors,
                    LoadBalancingSelectorType = LoadBalancingSelectorType
                };

                var result = client.GatewaysV2.PatchGatewayCluster(GatewayClusterId, request, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
