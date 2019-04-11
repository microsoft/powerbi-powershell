using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayTenantPolicyTests
    {
        [TestMethod]
        public async Task GetOnPremisesDataGatewayTenantPolicyJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var tenantId = "69BFBD2A-1901-48BA-9CAF-0D9190BEE34A";
            var serializedODataRepsonse = $@"{{
  ""@odata.context"":""http://example.net/v2.0/myorg/gatewayPolicy"",
  ""id"":""{tenantId}"",
  ""policy"":0
}}";

            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetTenantPolicy();

            // Assert
            result.TenantObjectId.Should().Be(tenantId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayTenantPolicyCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var tenantId = "69BFBD2A-1901-48BA-9CAF-0D9190BEE34A";
            var oDataResponse = new ODataResponseGatewayTenant
            {
                ODataContext = "http://example.net/v2.0/myorg/gatewayPolicy",
                TenantObjectId = tenantId,
                Policy = TenantPolicy.None
            };

            var serializedODataRepsonse = JsonConvert.SerializeObject(oDataResponse);
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetTenantPolicy();

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }
    }
}
