using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class SetOnPremisesDataGatewayInstallersTests
    {
        [TestMethod]
        public async Task SetOnPremisesDataGatewayInstallersCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayInstallersRequest
            {
                Ids = new string[]
                {
                    "id"
                },
                Operation = OperationType.None,
                GatewayType = GatewayType.Personal
            };

            // Act
            var result = await client.UpdateInstallerPrincipals(request);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
