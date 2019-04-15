using System;
using System.Management.Automation;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class RemoveOnPremisesDataGatewayClusterUserTests
    {
        private static CmdletInfo RemoveOnPremisesDataGatewayClusterUserInfo { get; } = new CmdletInfo(
            $"{RemoveOnPremisesDataGatewayClusterUser.CmdletVerb}-{RemoveOnPremisesDataGatewayClusterUser.CmdletName}",
            typeof(RemoveOnPremisesDataGatewayClusterUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemoveOnPremisesDataGatewayClusterUser()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(RemoveOnPremisesDataGatewayClusterUserInfo)
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterUser.GatewayClusterId), new Guid())
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterUser.PrincipalObjectId), new Guid("{F31FEA72-8435-4871-BF75-E94168C71A6D}"));

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }
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
