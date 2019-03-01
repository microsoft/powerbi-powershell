using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public class GatewayPermission
    {
        public enum GatewayPrincipalType {
            User
        }


        public Guid Id { get; set; }
        public GatewayPrincipalType PrincipalType { get; set; }
        public Guid TenantId { get; set; }

        {
                    "id": "cbf99e2e-a04a-4e24-ba8e-2da95260760c",
                    "principalType": "User",
                    "tenantId": "6c21bb0c-b94d-4915-9d1b-b505dcc51a3a",
                    "role": "Admin",
                    "allowedDataSources": [],
                    "clusterId": "41902731-56f6-48b8-b381-945a6ec5ac5d"
                }
}
}
