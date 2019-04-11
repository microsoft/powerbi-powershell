using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class PatchGatewayClusterRequest
    {
        [DataMember(IsRequired = false, Name = "name")]
        public string Name { get; set; }

        [DataMember(IsRequired = false, Name = "department")]
        public string Department { get; set; }

        [DataMember(IsRequired = false, Name = "description")]
        public string Description { get; set; }

        [DataMember(IsRequired = false, Name = "contactInformation")]
        public string ContactInformation { get; set; }

        [DataMember(IsRequired = false, Name = "allowCloudDatasourceRefresh")]
        public bool? AllowCloudDatasourceRefresh { get; set; }

        [DataMember(IsRequired = false, Name = "allowCustomConnectors")]
        public bool? AllowCustomConnectors { get; set; }

        [DataMember(IsRequired = false, Name = "loadBalancingSelectorType")]
        public string LoadBalancingSelectorType { get; set; }
    }
}
