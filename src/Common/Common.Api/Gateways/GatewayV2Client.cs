using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;
using Newtonsoft.Json;

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
            this.Token = tokenCredentials;

            HttpClientInstance = new HttpClient(messageHandler);
            PopulateClient(HttpClientInstance);
        }

        ~GatewayV2Client()
        {
            HttpClientInstance.Dispose();
        }


        private void PopulateClient(HttpClient client)
        {
            client.BaseAddress = this.BaseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.Token.AccessToken);
            client.DefaultRequestHeaders.UserAgent.Clear();
            //client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MicrosoftPowerBIMgmt-InvokeRest", PowerBICmdlet.CmdletVersion));
        }

        public async Task<IEnumerable<GatewayCluster>> GetGatewayClusters(bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += "/gatewayclusters?$expand=permissions,memberGateways";

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseList<GatewayCluster>));

            var clusters = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseList<GatewayCluster>;
            return clusters?.Value;
        }

        public async Task<IEnumerable<InstallerPrincipal>> GetInstallerPrincipals(GatewayType? type)
        {
            var url = $"v2.0/myorg/gatewayInstallers";
            if (type != null)
            {
                url += $"?type={type.ToString()}";
            }

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<InstallerPrincipal>));

            var installerPrincipal = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as IEnumerable<InstallerPrincipal>;
            return installerPrincipal;
        }

        public async Task<GatewayCluster> GetGatewayClusters(Guid gatewayClusterId, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayclusters({gatewayClusterId})?$expand=permissions,memberGateways";

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseGatewayCluster));

            var cluster = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseGatewayCluster;
            return cluster;
        }

        public async Task<GatewayClusterStatusResponse> GetGatewayClusterStatus(Guid gatewayClusterId, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayclusters({gatewayClusterId})/status?$expand=permissions,memberGateways";

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseGatewayClusterStatusResponse));

            var clusterStatus = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseGatewayClusterStatusResponse;
            return clusterStatus;
        }

        public async Task<HttpResponseMessage> PatchGatewayCluster(Guid gatewayClusterId, PatchGatewayClusterRequest patchGatewayClusterRequest, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayclusters({gatewayClusterId})";

            var httpContent = new StringContent(JsonConvert.SerializeObject(patchGatewayClusterRequest), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PatchAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> DeleteGatewayCluster(Guid gatewayClusterId, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayclusters({gatewayClusterId})";

            return await HttpClientInstance.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> DeleteGatewayClusterMember(Guid gatewayClusterId, Guid memberGatewayId, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayclusters({gatewayClusterId})/memberGateways({memberGatewayId})";

            return await HttpClientInstance.DeleteAsync(url);

        }

        public async Task<HttpResponseMessage> AddUsersToGatewayCluster(Guid gatewayClusterId, GatewayClusterAddPrincipalRequest addPrincipalRequest, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayclusters({gatewayClusterId})/permissions";

            var httpContent = new StringContent(JsonConvert.SerializeObject(addPrincipalRequest), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> DeleteUserOnGatewayCluster(Guid gatewayClusterId, Guid permissionId, bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            url += $"/gatewayClusters({gatewayClusterId})/permissions({permissionId})";

            return await HttpClientInstance.DeleteAsync(url);
        }

        public async Task<GatewayTenant> GetTenantPolicy()
        {
            var url = "v2.0/myorg/gatewayPolicy";

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseGatewayTenant));

            var tenantPolicy = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseGatewayTenant;
            return tenantPolicy;
        }

        public async Task<HttpResponseMessage> UpdateTenantPolicy(UpdateGatewayPolicyRequest request)
        {
            var url = "v2.0/myorg/gatewayPolicy";

            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> UpdateInstallerPrincipals(UpdateGatewayInstallersRequest request)
        {
            var url = "v2.0/myorg/gatewayInstallers";

            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PostAsync(url, httpContent);
        }
    }
}
