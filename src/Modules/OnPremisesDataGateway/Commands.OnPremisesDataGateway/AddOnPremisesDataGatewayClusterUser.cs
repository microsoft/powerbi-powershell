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
    public class AddOnPremisesDataGatewayClusterUser : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletName = "OnPremisesDataGatewayClusterUser";
        public const string CmdletVerb = VerbsCommon.Add;

        [Parameter()]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Alias("Cluster")]
        [Parameter()]
        public Guid GatewayClusterId { get; set; }

        [Parameter()]
        public Guid PrincipalObjectId { get; set; }

        [Parameter(Mandatory = false)]
        public IEnumerable<DatasourceType> AllowedDataSourceTypes { get; set; }

        [Parameter()]
        public string Role { get; set; }

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

                var result = client.GatewaysV2.AddUsersToGatewayCluster(GatewayClusterId, request, this.Scope == PowerBIUserScope.Individual);
            }
        }
    }
}
