using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    public class ODataGatewayResponseList<T>
    {
        [JsonProperty]
        public string Odatacontext { get; set; }

        [JsonProperty]
        public IList<T> Value { get; set; }

        public ODataGatewayResponseList()
        {
        }

        public ODataGatewayResponseList(string odatacontext = null, IList<T> value = null)
        {
            Odatacontext = odatacontext;
            Value = value;
        }
    }
}
