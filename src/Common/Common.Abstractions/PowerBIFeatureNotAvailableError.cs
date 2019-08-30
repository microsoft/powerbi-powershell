using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Abstractions
{
    [DataContract]
    public class PowerBIFeatureNotAvailableError
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}
