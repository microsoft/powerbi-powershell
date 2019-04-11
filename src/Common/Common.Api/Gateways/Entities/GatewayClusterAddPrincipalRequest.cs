using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class GatewayClusterAddPrincipalRequest
    {
        [DataMember(Name = "id", IsRequired = true)]
        public Guid PrincipalObjectId { get; set; }

        [DataMember(Name = "allowedDataSourceTypes")]
        public IEnumerable<DatasourceType> AllowedDataSourceTypes { get; set; }

        [DataMember(Name = "role", IsRequired = true)]
        public string Role { get; set; }
    }
}
