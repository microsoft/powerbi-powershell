using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class ODataGatewayResponseList<T>
    {
        [DataMember(Name = "@odata.context")]
        public string OdataContext { get; set; }

        [DataMember(Name = "value")]
        public IEnumerable<T> Value { get; set; }
    }
}
