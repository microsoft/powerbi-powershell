using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    public enum PrivacyLevel
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Private = 1,

        [EnumMember]
        Organizational = 2,

        [EnumMember]
        Public = 3
    }
}
