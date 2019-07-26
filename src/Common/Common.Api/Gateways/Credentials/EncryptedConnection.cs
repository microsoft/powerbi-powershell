using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    public enum EncryptedConnection
    {
        [EnumMember]
        Encrypted = 0,

        [EnumMember]
        Any = 1,

        [EnumMember]
        NotEncrypted = 2
    }
}
