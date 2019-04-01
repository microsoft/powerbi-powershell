using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Common.Api.Gateways
{
    public class GatewayV2Client : IGatewayV2Client
    {
        private Uri BaseUri { get; set; }
        private IAccessToken Token { get; set; }


        public GatewayV2Client(Uri baseUri, IAccessToken tokenCredentials)
        {
            this.BaseUri = baseUri;
            this.Token = tokenCredentials;
        }

        public GatewayV2Client(Uri baseUri, IAccessToken tokenCredentials, HttpClientHandler clientHandler) : this(baseUri, tokenCredentials)
        {
            // implement clientHandler
        }

        public async Task<IEnumerable<GatewayCluster>> GetGatewayClusters(bool asIndividual)
        {
            var url = "v2.0/myorg";
            if(asIndividual)
            {
                url += "/me";
            }

            url += "/gatewayclusters$expand=permissions,memberGateways";
            using (var client = new HttpClient())
            {
                PopulateClient(client);
                var response = await client.GetAsync(url);
                var serializer = new DataContractJsonSerializer(typeof(ODataGatewayResponseList<GatewayCluster>));

                var gatewayClusters = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataGatewayResponseList<GatewayCluster>;
                return gatewayClusters?.Value;
            }
        }

        private void PopulateClient(HttpClient client)
        {
            client.BaseAddress = this.BaseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.Token.AccessToken);
            client.DefaultRequestHeaders.UserAgent.Clear();
            //client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MicrosoftPowerBIMgmt-InvokeRest", PowerBICmdlet.CmdletVersion));
        }
    }
}
