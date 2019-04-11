using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class GatewayTenant
    {
        [DataMember(Name = "id")]
        public string TenantObjectId { get; set; }

        [DataMember(Name = "policy")]
        public TenantPolicy Policy { get; set; }
    }
}
