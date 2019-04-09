using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class Permission
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "principalType")]
        public string PrincipalType { get; set; }

        [DataMember(Name = "tenantId")]
        public string TenantId { get; set; }

        [DataMember(Name = "role")]
        public string Role { get; set; }

        [DataMember(Name = "allowedDataSources")]
        public IEnumerable<string> AllowedDataSources { get; set; }

        [DataMember(Name = "clusterId")]
        public string ClusterId { get; set; }
    }
}
