using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class SetOnPremisesDataGatewayTenantPolicyTests
    {
        [TestMethod]
        public async Task SetOnPremisesDataGatewayTenantPolicyCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayPolicyRequest()
            {
                ResourceGatewayInstallPolicy = PolicyType.Restricted,
                PersonalGatewayInstallPolicy = PolicyType.None
            };

            // Act
            var result = await client.UpdateTenantPolicy(request);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
