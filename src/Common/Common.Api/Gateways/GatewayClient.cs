/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

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
using Newtonsoft.Json;

using static System.FormattableString;

namespace Microsoft.PowerBI.Common.Api.Gateways
{
    public class GatewayClient : IGatewayClient
    {
        private static readonly string CmdletVersion = typeof(GatewayClient).Assembly.GetName().Version.ToString();

        private Uri BaseUri { get; }
        private IAccessToken Token { get; }

        private HttpClient HttpClientInstance { get; }

        public GatewayClient(Uri baseUri, IAccessToken tokenCredentials) : this(baseUri, tokenCredentials, new HttpClientHandler())
        {
        }

        public GatewayClient(Uri baseUri, IAccessToken tokenCredentials, HttpMessageHandler messageHandler)
        {
            this.BaseUri = baseUri;
            this.Token = tokenCredentials;

            HttpClientInstance = new HttpClient(messageHandler);
            PopulateClient(HttpClientInstance);
        }

        ~GatewayClient()
        {
            HttpClientInstance.Dispose();
        }


        private void PopulateClient(HttpClient client)
        {
            client.BaseAddress = this.BaseUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token.AccessToken);
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MicrosoftPowerBIMgmt-Gw-InvokeRest", CmdletVersion));
        }

        private static string GetODataUrlStart(bool asIndividual)
        {
            var url = "v2.0/myorg";
            if (asIndividual)
            {
                url += "/me";
            }

            return url;
        }

        public async Task<IEnumerable<GatewayCluster>> GetGatewayClusters(bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseList<GatewayCluster>));

            var clusters = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseList<GatewayCluster>;
            return clusters?.Value;
        }

        public async Task<IEnumerable<InstallerPrincipal>> GetInstallerPrincipals(GatewayType? type)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayInstallers");
            if (type != null)
            {
                url += Invariant($"?type={type.ToString()}");
            }

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<InstallerPrincipal>));

            var installerPrincipal = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as IEnumerable<InstallerPrincipal>;
            return installerPrincipal;
        }

        public async Task<GatewayCluster> GetGatewayClusters(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseGatewayCluster));

            var cluster = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseGatewayCluster;
            return cluster;
        }

        public async Task<GatewayClusterStatusResponse> GetGatewayClusterStatus(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})/status?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseGatewayClusterStatusResponse));

            var clusterStatus = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseGatewayClusterStatusResponse;
            return clusterStatus;
        }

        public async Task<HttpResponseMessage> PatchGatewayCluster(Guid gatewayClusterId, PatchGatewayClusterRequest patchGatewayClusterRequest, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})");

            var httpContent = new StringContent(JsonConvert.SerializeObject(patchGatewayClusterRequest), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PatchAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> DeleteGatewayCluster(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})");

            return await HttpClientInstance.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> DeleteGatewayClusterMember(Guid gatewayClusterId, Guid memberGatewayId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})/memberGateways({memberGatewayId})");

            return await HttpClientInstance.DeleteAsync(url);

        }

        public async Task<HttpResponseMessage> AddUsersToGatewayCluster(Guid gatewayClusterId, GatewayClusterAddPrincipalRequest addPrincipalRequest, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})/permissions");

            var httpContent = new StringContent(JsonConvert.SerializeObject(addPrincipalRequest), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> DeleteUserOnGatewayCluster(Guid gatewayClusterId, Guid permissionId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters({gatewayClusterId})/permissions({permissionId})");

            return await HttpClientInstance.DeleteAsync(url);
        }

        public async Task<GatewayTenant> GetTenantPolicy()
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayPolicy");

            var response = await HttpClientInstance.GetAsync(url);
            var serializer = new DataContractJsonSerializer(typeof(ODataResponseGatewayTenant));

            var tenantPolicy = serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as ODataResponseGatewayTenant;
            return tenantPolicy;
        }

        public async Task<HttpResponseMessage> UpdateTenantPolicy(UpdateGatewayPolicyRequest request)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayPolicy");

            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> UpdateInstallerPrincipals(UpdateGatewayInstallersRequest request)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayInstallers");

            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            return await HttpClientInstance.PostAsync(url, httpContent);
        }
    }
}
