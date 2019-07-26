using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    [DataContract]
    public enum CredentialType
    {
        [EnumMember]
        NotSpecified = 0,

        [EnumMember]
        Windows = 1,

        [EnumMember]
        Anonymous = 2,

        [EnumMember]
        Basic = 3,

        [EnumMember]
        Key = 4,

        [EnumMember]
        OAuth2 = 5,

        [EnumMember]
        EffectiveUserName = 6,

        [EnumMember]
        SingleSignOn = 7,

        [EnumMember]
        WindowsWithoutImpersonation = 8,
    }
}
