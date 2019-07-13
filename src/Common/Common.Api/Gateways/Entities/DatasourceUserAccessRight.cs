using System;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    [Flags]
    public enum DatasourceUserAccessRight
    {
        None                          = 0x0,
        Read                          = 0x1,
        Write                         = 0x2,
        Admin                         = 0x4,
        ReadWrite                     = Read | Write,
        OverrideEffectiveIdentity     = 0x8,
        ReadOverrideEffectiveIdentity = Read | OverrideEffectiveIdentity,
    }
}
