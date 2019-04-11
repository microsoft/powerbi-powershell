using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var serializedODataRepsonse = $@"{{
  ""@odata.context"":""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/status/$entity"",
  ""clusterStatus"":""{clusterStatus}"",
  ""gatewayStaticCapabilities"":""the static capabilities"",
  ""gatewayVersion"":""3000.0.0.0+gabcdef0"",
  ""gatewayUpgradeState"":""the upgrade state""
}}";

            var client = Utilities.GetTestClient(serializedODataRepsonse);

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
            var oDataResponse = new ODataResponseGatewayClusterStatusResponse
            {
                ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/status/$entity",
                ClusterStatus = clusterStatus,
                GatewayStaticCapabilities = "the static capabilities",
                GatewayVersion = "3000.0.0.0+gabcdef0",
                GatewayUpgradeState = "the upgrade state"
            };

            var serializedODataRepsonse = JsonConvert.SerializeObject(oDataResponse);
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetGatewayClusterStatus(new Guid(), true);

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }
    }
}
