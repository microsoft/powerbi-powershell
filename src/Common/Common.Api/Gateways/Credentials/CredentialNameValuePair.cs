using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    [DataContract]
    public sealed class CredentialNameValuePair
    {
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        [DataMember(Order = 20, Name = "value")]
        public string Value { get; set; }
    }
}
