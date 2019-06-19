/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(HttpStatusCode))]
    public class AddOnPremisesDataGatewayClusterUser : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterUser";
        public const string CmdletVerb = VerbsCommon.Add;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Alias("User")]
        [Parameter(Mandatory = true)]
        public Guid PrincipalObjectId { get; set; }

        [Parameter()]
        public List<DatasourceType> AllowedDataSourceTypes { get; set; }

        [Parameter(Mandatory = true)]
        public string Role { get; set; }

        public AddOnPremisesDataGatewayClusterUser() : base() { }

        public AddOnPremisesDataGatewayClusterUser(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new GatewayClusterAddPrincipalRequest
                {
                    PrincipalObjectId = PrincipalObjectId,
                    AllowedDataSourceTypes = AllowedDataSourceTypes,
                    Role = Role
                };

                _ = client.Gateways.AddUsersToGatewayCluster(GatewayClusterId, request, this.Scope == PowerBIUserScope.Individual).Result;
            }
        }
    }
}
