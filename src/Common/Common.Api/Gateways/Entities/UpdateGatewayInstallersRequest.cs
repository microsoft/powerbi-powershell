using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class UpdateGatewayInstallersRequest
    {
        [Required]
        [DataMember(Name = "ids")]
        public IEnumerable<string> Ids { get; set; } = new string[0];

        [Required]
        [DataMember(Name = "operation")]
        public OperationType Operation { get; set; }

        [Required]
        [DataMember(Name = "gatewayType")]
        public GatewayType GatewayType { get; set; }
    }
}
