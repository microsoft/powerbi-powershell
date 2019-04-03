using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;

namespace Microsoft.PowerBI.Common.Api.Gateways
{
    public class GatewayV2Client : IGatewayV2Client
    {
        private Uri BaseUri { get; }
        private IAccessToken Token { get; }
        private HttpClient HttpClientInstance { get; }

        public GatewayV2Client(Uri baseUri, IAccessToken tokenCredentials) : this(baseUri, tokenCredentials, new HttpClientHandler())
        {
        }

        public GatewayV2Client(Uri baseUri, IAccessToken tokenCredentials, HttpMessageHandler messageHandler)
        {
            this.BaseUri = baseUri;
            this.Token   = tokenCredentials;

            HttpClientInstance = new HttpClient(messageHandler);
            PopulateClient(HttpClientInstance);
        }

        public async Task<IEnumerable<GatewayCluster>> GetGatewayClusters(bool asIndividual)
        {
            var url = "v2.0/myorg";
            if(asIndividual)
            {
                url += "/me";
            }

            url += "/gatewayclusters?$expand=permissions,memberGateways";
            using (HttpClientInstance)
            {
                var response = await HttpClientInstance.GetAsync(url);
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
