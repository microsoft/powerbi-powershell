using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    [DataContract]
    public class CredentialData
    {
        [DataMember(Order = 10, Name = "credentialData")]
        public IList<CredentialNameValuePair> CredentialNameValuePairs { get; set; }
    }
}