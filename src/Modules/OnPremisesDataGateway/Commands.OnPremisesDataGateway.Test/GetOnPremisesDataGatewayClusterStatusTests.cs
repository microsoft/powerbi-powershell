using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayClusterStatusTests
    {
        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersWithClusterIdJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var clusterStatus = "the cluster status";
            var client = new GatewayV2Client(
                new Uri("https://bing.com"),
                new Mock<IAccessToken>().Object,
                new MockHttpMessageHandler
                {
                    SendAsyncMockHandler = (httpMessageRequest, cancellationToken) =>
                    {
                        var content = $@"{{
  ""@odata.context"":""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/status/$entity"",
  ""clusterStatus"":""{clusterStatus}"",
  ""gatewayStaticCapabilities"":""the static capabilities"",
  ""gatewayVersion"":""3000.0.0.0+gabcdef0"",
  ""gatewayUpgradeState"":""the upgrade state""
}}
";
                        var response = httpMessageRequest.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(
                            content,
                            Encoding.UTF8,
                            "application/json");
                        return response;
                    }
                });

            // Act
            var result = await client.GetGatewayClusterStatus(new Guid(), true);

            // Assert
            result.ClusterStatus.Should().Be(clusterStatus);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterStatusCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterStatus = "the cluster status";
            var odataResponse = new ODataResponseGatewayClusterStatusResponse
            {
                ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/status/$entity",
                ClusterStatus = clusterStatus,
                GatewayStaticCapabilities = "the static capabilities",
                GatewayVersion = "3000.0.0.0+gabcdef0",
                GatewayUpgradeState = "the upgrade state"
            };

            var serializedOdataRepsonse = JsonConvert.SerializeObject(odataResponse);

            var client = new GatewayV2Client(
                new Uri("https://bing.com"),
                new Mock<IAccessToken>().Object,
                new MockHttpMessageHandler
                {
                    SendAsyncMockHandler = (httpMessageRequest, cancellationToken) =>
                    {
                        var content = serializedOdataRepsonse;
                        var response = httpMessageRequest.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(
                            content,
                            Encoding.UTF8,
                            "application/json");
                        return response;
                    }
                });

            // Act
            var result = await client.GetGatewayClusterStatus(new Guid(), true);

            // Assert
            odataResponse.Should().BeEquivalentTo(result);
        }
    }
}
