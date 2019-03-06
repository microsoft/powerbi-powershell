using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public sealed class MemberGateway
    {
        public string Id { get; set; }
        public string PrincipalType { get; set; }
        public string TenantId { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> AllowedDataSources { get; set; }
        public string ClusterId { get; set; }
    }
}