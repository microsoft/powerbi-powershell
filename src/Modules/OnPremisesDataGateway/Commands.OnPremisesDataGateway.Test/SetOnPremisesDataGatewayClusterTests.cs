using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class SetOnPremisesDataGatewayClusterTests
    {
        [TestMethod]
        public async Task SetOnPremisesDataGatewayClusterCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new PatchGatewayClusterRequest()
            {
                Name = "name",
                Department = "department",
                Description = "description",
                ContactInformation = "contactInformation",
                AllowCloudDatasourceRefresh = true,
                AllowCustomConnectors = true,
                LoadBalancingSelectorType = "loadBalancingSelectorType"
            };

            // Act
            var result = await client.PatchGatewayCluster(new Guid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
