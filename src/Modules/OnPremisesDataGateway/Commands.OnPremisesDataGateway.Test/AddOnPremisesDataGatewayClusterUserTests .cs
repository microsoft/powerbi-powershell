using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class AddOnPremisesDataGatewayClusterUserTests
    {
        [TestMethod]
        public async Task AddOnPremisesDataGatewayClusterUserCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new GatewayClusterAddPrincipalRequest()
            {
                PrincipalObjectId = new Guid(),
                AllowedDataSourceTypes = new DatasourceType[] 
                {
                    DatasourceType.Sql
                },
                Role = "the role"

            };

            // Act
            var result = await client.AddUsersToGatewayCluster(new Guid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
