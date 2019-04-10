using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class ODataGatewayClusterResponse :  GatewayCluster
    {
        [DataMember(Name = "@odata.context")]
        public string OdataContext { get; set; }
    }
}
