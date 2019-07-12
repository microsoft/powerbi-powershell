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
    [OutputType(typeof(GatewayClusterDatasource))]
    public class GetOnPremisesDataGatewayClusterDatasource : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterDatasource";
        public const string CmdletVerb = VerbsCommon.Get;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Alias("DatasourceId", "Datasource")]
        [Parameter()]
        public Guid GatewayClusterDatasourceId { get; set; }

        public GetOnPremisesDataGatewayClusterDatasource() : base() { }

        public GetOnPremisesDataGatewayClusterDatasource(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                if (this.GatewayClusterDatasourceId != default)
                {
                    var result = client.Gateways.GetGatewayClusterDatasource(
                                            GatewayClusterId,
                                            this.GatewayClusterDatasourceId,
                                            this.Scope == PowerBIUserScope.Individual)
                                       .Result;
                    Logger.WriteObject(result, true);
                }
                else
                {
                    var result = client.Gateways.GetGatewayClusterDatasources(
                                            GatewayClusterId,
                                            this.Scope == PowerBIUserScope.Individual)
                                       .Result;
                    Logger.WriteObject(result, true);
                }
            }
        }
    }
}
