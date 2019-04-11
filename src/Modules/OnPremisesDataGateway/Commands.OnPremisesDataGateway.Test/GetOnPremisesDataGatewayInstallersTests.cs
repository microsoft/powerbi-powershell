using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayInstallersTests
    {
        [TestMethod]
        public async Task GetOnPremisesDataGatewayInstallersJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var principalObjectId = "836083DE-E806-44E0-9C0D-53BCFD3800CB";
            var serializedODataRepsonse = $@"[
  {{
    ""id"":""{principalObjectId}"",
    ""type"":""Personal""
  }}
]";

            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetInstallerPrincipals(GatewayType.Personal);

            // Assert
            result.ToArray()[0].PrincipalObjectId.Should().Be(principalObjectId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayInstallersCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var principalObjectId = "836083DE-E806-44E0-9C0D-53BCFD3800CB";
            var oDataResponse = new InstallerPrincipal[]
            {
                new InstallerPrincipal{
                    PrincipalObjectId = principalObjectId,
                    GatewayType = GatewayType.Personal.ToString()
                }
            };

            var serializedODataRepsonse = JsonConvert.SerializeObject(oDataResponse);
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetInstallerPrincipals(GatewayType.Personal);

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }
    }
}

