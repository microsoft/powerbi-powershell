using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Abstractions
{
    public class PowerBIEnvironment : IPowerBIEnvironment
    {
        public PowerBIEnvironmentType Name { get; set; }
        public string AzureADAuthority { get; set; }
        public string AzureADResource { get; set; }
        public string AzureADClientId { get; set; }
        public string AzureADRedirectAddress { get; set; }
        public string GlobalServiceEndpoint { get; set; }
        public string BackendUriOverride { get; set; }
    }
}