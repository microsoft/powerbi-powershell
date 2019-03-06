using System;
using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public sealed class GatewayCluster
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
        public IEnumerable<Guid> DataSourceIds { get; set; }
        public IEnumerable<Guid> AppIds { get; set; }
        public IEnumerable<MemberGateway> MemberGateways { get; set; }
    }
}
