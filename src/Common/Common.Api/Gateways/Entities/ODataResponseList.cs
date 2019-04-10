using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class ODataResponseList<T>
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }

        [DataMember(Name = "value")]
        public IEnumerable<T> Value { get; set; }
    }
}
