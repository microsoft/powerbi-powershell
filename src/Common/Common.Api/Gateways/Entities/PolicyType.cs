using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public enum PolicyType
    {
        [EnumMember] None,
        [EnumMember] Open,
        [EnumMember] Restricted,
    }
}
