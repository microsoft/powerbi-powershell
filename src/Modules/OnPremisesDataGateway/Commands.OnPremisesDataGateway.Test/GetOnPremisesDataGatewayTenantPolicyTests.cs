using System;
using System.Management.Automation;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayTenantPolicyTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayTenantPolicyInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayTenantPolicy.CmdletVerb}-{GetOnPremisesDataGatewayTenantPolicy.CmdletName}",
            typeof(GetOnPremisesDataGatewayTenantPolicy));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayTenantPolicy()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayTenantPolicyInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

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
