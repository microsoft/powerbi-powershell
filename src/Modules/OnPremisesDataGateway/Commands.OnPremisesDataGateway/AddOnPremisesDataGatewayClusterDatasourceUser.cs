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
    public class AddOnPremisesDataGatewayClusterDatasourceUser : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterDatasourceUser";
        public const string CmdletVerb = VerbsCommon.Add;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster", "Id")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterId { get; set; }

        [Alias("DatasourceId")]
        [Parameter(Mandatory = true)]
        public Guid GatewayDatasourceId { get; set; }

        [Parameter(Mandatory = true)]
        public string DatasourceUserAccessRight { get; set; }

        [Parameter()]
        public string DisplayName { get; set; }

        [Parameter()]
        public string Identifier { get; set; }

        [Parameter()]
        public string PrincipalType { get; set; }

        [Parameter()]
        public string EmailAddress { get; set; }

        public AddOnPremisesDataGatewayClusterDatasourceUser() : base() { }

        public AddOnPremisesDataGatewayClusterDatasourceUser(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new DatasourceUser
                {
                    DatasourceUserAccessRight = DatasourceUserAccessRight,
                    DisplayName = DisplayName,
                    Identifier = Identifier,
                    PrincipalType = PrincipalType,
                    EmailAddress = EmailAddress,
                };

                var result = client.Gateways.AddUsersToGatewayClusterDatasource(GatewayClusterId, GatewayDatasourceId, request, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteObject(result, true);
            }
        }
    }
}

