using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Abstractions
{
    public class PowerBIProfile : IPowerBIProfile
    {
        public IPowerBIEnvironment Environment { get; }

        public string TenantId { get; }

        public string UserName { get; }

        public PowerBIProfileType LoginType { get; }

        public PowerBIProfile(IPowerBIEnvironment environment, IAccessToken token, PowerBIProfileType profileType) => 
            (this.Environment, this.TenantId, this.UserName, this.LoginType) = (environment, token.TenantId, token.UserName, profileType);
    }
}