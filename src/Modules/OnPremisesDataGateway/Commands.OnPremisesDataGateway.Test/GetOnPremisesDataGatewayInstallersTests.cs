using System.Linq;
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
    public class GetOnPremisesDataGatewayInstallersTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayInstallersInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayInstallers.CmdletVerb}-{GetOnPremisesDataGatewayInstallers.CmdletName}",
            typeof(GetOnPremisesDataGatewayInstallers));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayInstallers()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayInstallersInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

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

