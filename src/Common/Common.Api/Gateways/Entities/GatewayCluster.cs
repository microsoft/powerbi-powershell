using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public class GatewayCluster
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> DataSourceIds { get; set; }
        public IEnumerable<Guid> AppIds { get; set; }
        public IEnumerable<GatewayPermission> Permissions { get; set; }
        public IEnumerable<Gateway> MemberGateways { get; set; }

            "name": "Test1",
            "dataSourceIds": [],
            "appIds": [],
            "permissions": [
                
            ],
            "memberGateways": [
                {
                    "id": "41902731-56f6-48b8-b381-945a6ec5ac5d",
                    "name": "Test1",
                    "type": "Resource",
                    "version": "3000.1.257",
                    "annotation": "{\"gatewayContactInformation\":[\"admin@granularcontrols1.ccsctp.net\"],\"gatewayVersion\":\"3000.1.257+g34ac4c1d11\",\"gatewayWitnessString\":\"{\\\"EncryptedResult\\\":\\\"aN1eMeuuYvFik+fS5kCwb1ON6XS08GRmakMhO25jeRvUj/JtHSlwgNhHItBlG+n4unmvNMCRA6zIgLzs5M+IXJFJV95wHVbpWBkWXBJf9WKtIxBvgOfVvU0Dea5qqUzb6RWGl3gKRAsC+U1z/fOZAQ==\\\",\\\"IV\\\":\\\"FKUVfWlK8gnFX1+FHMxpgA==\\\",\\\"Signature\\\":\\\"TXiPzs1AAvJJl1C5jZ0g8X1tEw5IHxVGrkWSugZYmec=\\\"}\",\"gatewayMachine\":\"chbeck001\"}",
                    "clusterId": "41902731-56f6-48b8-b381-945a6ec5ac5d"
                }
            ]
        }
    ]
    }
}
