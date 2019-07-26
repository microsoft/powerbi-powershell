/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Abstractions.Utilities;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var clusters = await DeserializeResponseContent<ODataResponseList<GatewayCluster>>(response);

            return clusters?.Value;
        }

        public async Task<IEnumerable<InstallerPrincipal>> GetInstallerPrincipals(GatewayType? type)
        {
            var url = Invariant($"{GetODataUrlStart(false)}/gatewayInstallers");
            if (type != null)
            {
                var encodedOdataFilter = HttpUtility.UrlEncode(Invariant($"type eq '{type.ToString()}'"));
                url += Invariant($"?$filter={encodedOdataFilter}");
            }

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var odataInstallerPrincipal = await DeserializeResponseContent<ODataResponseList<InstallerPrincipal>>(response);

            return odataInstallerPrincipal.Value;
        }

        public async Task<GatewayCluster> GetGatewayCluster(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var cluster = await DeserializeResponseContent<ODataResponseGatewayCluster>(response);

            return cluster;
        }

        public async Task<GatewayClusterStatusResponse> GetGatewayClusterStatus(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/status?$expand=permissions,memberGateways");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var clusterStatus = await DeserializeResponseContent<ODataResponseGatewayClusterStatusResponse>(response);

            return clusterStatus;
        }

        public async Task<HttpResponseMessage> PatchGatewayCluster(Guid gatewayClusterId, PatchGatewayClusterRequest patchGatewayClusterRequest, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}");

            var httpContent = SerializeObject(patchGatewayClusterRequest);
            var response = await HttpClientInstance.PatchAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteGatewayCluster(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteGatewayClusterMember(Guid gatewayClusterId, Guid memberGatewayId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/memberGateways/{memberGatewayId}");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> AddUsersToGatewayCluster(Guid gatewayClusterId, GatewayClusterAddPrincipalRequest addPrincipalRequest, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/permissions");

            var httpContent = SerializeObject(addPrincipalRequest);
            var response =  await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteUserOnGatewayCluster(Guid gatewayClusterId, Guid permissionId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/permissions/{permissionId}");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<GatewayTenant> GetTenantPolicy()
        {
            var url = Invariant($"{GetODataUrlStart(false)}/gatewayPolicy");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var tenantPolicy = await DeserializeResponseContent<ODataResponseGatewayTenant>(response);

            return tenantPolicy;
        }

        public async Task<HttpResponseMessage> UpdateTenantPolicy(UpdateGatewayPolicyRequest request)
        {
            var url = Invariant($"{GetODataUrlStart(false)}/gatewayPolicy");

            var httpContent = SerializeObject(request);
            var response = await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> UpdateInstallerPrincipals(UpdateGatewayInstallersRequest request)
        {
            var url = Invariant($"{GetODataUrlStart(false)}/gatewayInstallers");

            var httpContent = SerializeObject(request);
            var response = await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<IEnumerable<GatewayClusterDatasource>> GetGatewayClusterDatasources(Guid gatewayClusterId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/datasources");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var gatewayClusterDatasources = await DeserializeResponseContent<ODataResponseList<GatewayClusterDatasource>>(response);

            return gatewayClusterDatasources?.Value;
        }

        public async Task<GatewayClusterDatasource> GetGatewayClusterDatasource(Guid gatewayClusterId, Guid gatewayClusterDatasourceId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var gatewayClusterDatasource = await DeserializeResponseContent<ODataResponseGatewayClusterDatasource>(response);

            return gatewayClusterDatasource;
        }

        public async Task<HttpResponseMessage> GetGatewayClusterDatasourceStatus(Guid gatewayClusterId, Guid gatewayClusterDatasourceId, bool asIndividual)
        {

            var url = Invariant($"{GetODataUrlStart(asIndividual)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}/status");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<GatewayClusterDatasource> CreateGatewayClusterDatasource(
            Guid gatewayClusterId,
            PublishDatasourceToGatewayClusterRequest datasourceToGatewayClusterRequest,
            bool asIndividual)
        {

            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources");

            var httpContent = SerializeObject(datasourceToGatewayClusterRequest);
            var response = await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            var gatewayClusterDatasource = await DeserializeResponseContent<ODataResponseGatewayClusterDatasource>(response);

            return gatewayClusterDatasource;
        }

        public async Task<HttpResponseMessage> UpdateGatewayClusterDatasource(
            Guid gatewayClusterId,
            Guid gatewayClusterDatasourceId,
            UpdateGatewayClusterDatasourceRequest updateDatasourceRequest,
            bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}");

            var httpContent = SerializeObject(updateDatasourceRequest);
            var response = await HttpClientInstance.PatchAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteGatewayClusterDatasource(Guid gatewayClusterId, Guid gatewayClusterDatasourceId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> AddUsersToGatewayClusterDatasource(Guid gatewayClusterId, Guid gatewayClusterDatasourceId, UserAccessRightEntry datasourceUser, bool asIndividual)
        {

            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}/users");

            var httpContent = SerializeObject(datasourceUser);
            var response = await HttpClientInstance.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<IEnumerable<UserAccessRightEntry>> GetGatewayClusterDatasourceUsers(Guid gatewayClusterId, Guid gatewayClusterDatasourceId, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}/users");

            var response = await HttpClientInstance.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var datasourceUsers = await DeserializeResponseContent<ODataResponseList<UserAccessRightEntry>>(response);

            return datasourceUsers?.Value;
        }

        public async Task<HttpResponseMessage> RemoveGatewayClusterDatasourceUser(Guid gatewayClusterId, Guid gatewayClusterDatasourceId, string user, bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}/users/{user}");

            var response = await HttpClientInstance.DeleteAsync(url);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<IEnumerable<DatasourceErrorDetails>> UpdateGatewayClusterDatasourceCredentials(
            Guid gatewayClusterId,
            Guid gatewayClusterDatasourceId,
            DatasourceCredentialDetails updateCredentialDetails,
            bool asIndividual)
        {
            var url = Invariant($"{GetODataUrlStart(true)}/gatewayClusters/{gatewayClusterId}/datasources/{gatewayClusterDatasourceId}/credentials");

            var httpContent = SerializeObject(updateCredentialDetails);
            var response = await HttpClientInstance.PatchAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            var datasourceErrorDetailsList = await DeserializeResponseContent<ODataResponseList<DatasourceErrorDetails>>(response);

            return datasourceErrorDetailsList?.Value;
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
            return new StringContent(JsonConvert.SerializeObject(objectToSerialize, new StringEnumConverter()), Encoding.UTF8, "application/json");
        }

        private async Task<T> DeserializeResponseContent<T>(HttpResponseMessage response)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());

            using (var sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
