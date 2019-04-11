using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class ODataResponseGatewayTenant : GatewayTenant
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }
    }
}
