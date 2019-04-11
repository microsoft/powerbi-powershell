using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public enum TenantPolicy
    {
        [EnumMember] None = 0,
        [EnumMember] PersonalInstallRestricted = 1,
        [EnumMember] EnterpriseInstallRestricted = 2,
    }
}
