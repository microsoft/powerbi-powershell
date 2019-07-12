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
    public class GetOnPremisesDataGatewayClusterDatasourceUsers : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterDatasourceUsers";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Alias("DatasourceId", "Datasource")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterDatasourceId { get; set; }

        public GetOnPremisesDataGatewayClusterDatasourceUsers() : base() { }

        public GetOnPremisesDataGatewayClusterDatasourceUsers(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var result = client.Gateways.GetGatewayClusterDatasourceUsers(GatewayClusterId, GatewayClusterDatasourceId, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
