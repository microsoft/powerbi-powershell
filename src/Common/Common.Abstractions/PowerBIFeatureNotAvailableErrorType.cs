using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    [DataContract]
    public class PowerBIFeatureNotAvailableErrorType
    {
        [DataMember(Name = "error")]
        public PowerBIFeatureNotAvailableError Error { get; set; }
    }
}
