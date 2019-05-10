/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions.Utilities;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

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

        public async Task<IEnumerable<GatewayCluster>> GetGatewayClusters(bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var clusters = await DeserializeResponseContent<ODataResponseList<GatewayCluster>>(response);

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
            response.EnsureSuccessStatusCode();

            var installerPrincipal = await DeserializeResponseContent<IEnumerable<InstallerPrincipal>>(response);

            return installerPrincipal;
        }

        public async Task<GatewayCluster> GetGatewayClusters(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var cluster = await DeserializeResponseContent<ODataResponseGatewayCluster>(response);

            return cluster;
        }

        public async Task<GatewayClusterStatusResponse> GetGatewayClusterStatus(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})/status?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var clusterStatus = await DeserializeResponseContent<ODataResponseGatewayClusterStatusResponse>(response);

            return clusterStatus;
        }

        public async Task<HttpResponseMessage> PatchGatewayCluster(Guid gatewayClusterId, PatchGatewayClusterRequest patchGatewayClusterRequest, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})");

            var httpContent = SerializeObject(patchGatewayClusterRequest);
            var response = await HttpClientInstance.PatchAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteGatewayCluster(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteGatewayClusterMember(Guid gatewayClusterId, Guid memberGatewayId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})/memberGateways({memberGatewayId})");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> AddUsersToGatewayCluster(Guid gatewayClusterId, GatewayClusterAddPrincipalRequest addPrincipalRequest, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayclusters({gatewayClusterId})/permissions");

            var httpContent = SerializeObject(addPrincipalRequest);
            var response =  await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteUserOnGatewayCluster(Guid gatewayClusterId, Guid permissionId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters({gatewayClusterId})/permissions({permissionId})");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<GatewayTenant> GetTenantPolicy()
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayPolicy");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var tenantPolicy = await DeserializeResponseContent<ODataResponseGatewayTenant>(response);

            return tenantPolicy;
        }

        public async Task<HttpResponseMessage> UpdateTenantPolicy(UpdateGatewayPolicyRequest request)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayPolicy");

            var httpContent = SerializeObject(request);
            var response = await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> UpdateInstallerPrincipals(UpdateGatewayInstallersRequest request)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual: false)}/gatewayInstallers");

            var httpContent = SerializeObject(request);
            var response = await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
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

        private static StringContent SerializeObject(object objectToSerialize)
        {
            return new StringContent(JsonConvert.SerializeObject(objectToSerialize), Encoding.UTF8, "application/json");
        }

        private async Task<T> DeserializeResponseContent<T>(HttpResponseMessage response)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());

            using (var sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
