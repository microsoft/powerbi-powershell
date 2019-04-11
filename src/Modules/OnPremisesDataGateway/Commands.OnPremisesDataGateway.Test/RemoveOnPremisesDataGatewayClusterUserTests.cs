using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class RemoveOnPremisesDataGatewayClusterUserTests
    {
        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterUserCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.DeleteUserOnGatewayCluster(new Guid(), new Guid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
