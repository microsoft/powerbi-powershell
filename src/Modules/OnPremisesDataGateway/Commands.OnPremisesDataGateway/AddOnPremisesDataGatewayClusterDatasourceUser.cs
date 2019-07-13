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

        [Alias("DatasourceId", "Datasource")]
        [Parameter(Mandatory = true)]
        public Guid GatewayClusterDatasourceId { get; set; }

        [Parameter(Mandatory = true)]
        public DatasourceUserAccessRight? DatasourceUserAccessRight { get; set; }

        [Parameter()]
        public string DisplayName { get; set; }

        // Based on backend code, this input is redundant with UserEmailAddress
        //[Parameter()]
        //public string Identifier { get; set; }

        // Not currently used by this API
        //[Parameter()]
        //public PrincipalType? PrincipalType { get; set; }

        [Alias("User", "EmailAddress")]
        [Parameter(Mandatory = true)]
        public string UserEmailAddress { get; set; }

        public AddOnPremisesDataGatewayClusterDatasourceUser() : base() { }

        public AddOnPremisesDataGatewayClusterDatasourceUser(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = CreateClient())
            {
                var request = new UserAccessRightEntry
                {
                    DatasourceUserAccessRight = DatasourceUserAccessRight,
                    DisplayName = DisplayName,
                    UserEmailAddress = UserEmailAddress,
                };

                var result = client.Gateways.AddUsersToGatewayClusterDatasource(GatewayClusterId, GatewayClusterDatasourceId, request, this.Scope == PowerBIUserScope.Individual).Result;
                Logger.WriteDebug(result);
            }
        }
    }
}

