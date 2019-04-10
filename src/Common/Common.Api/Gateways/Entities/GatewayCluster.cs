using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class GatewayCluster
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "region")]
        public string Region { get; set; }

        [DataMember(Name = "permissions")]
        public IEnumerable<Permission> Permissions { get; set; }

        [DataMember(Name = "dataSourceIds")]
        public IEnumerable<Guid> DataSourceIds { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "memberGateways")]
        public IEnumerable<MemberGateway> MemberGateways { get; set; }
    }
}
