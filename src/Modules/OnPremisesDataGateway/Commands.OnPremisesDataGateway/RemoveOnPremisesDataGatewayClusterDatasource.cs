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
    public class RemoveOnPremisesDataGatewayClusterDatasource : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterDatasource";
        public const string CmdletVerb = VerbsCommon.Remove;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Alias("DatasourceId")]
        [Parameter(Mandatory = true)]
        public Guid GatewayDatasourceId { get; set; }

        public RemoveOnPremisesDataGatewayClusterDatasource() : base() { }

        public RemoveOnPremisesDataGatewayClusterDatasource(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var result = client.Gateways.DeleteGatewayClusterDatasource(GatewayClusterId, GatewayDatasourceId, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}
