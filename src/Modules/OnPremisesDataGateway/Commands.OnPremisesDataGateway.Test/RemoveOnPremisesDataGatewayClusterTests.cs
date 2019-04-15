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
    public class RemoveOnPremisesDataGatewayClusterTests
    {
        private static CmdletInfo RemoveOnPremisesDataGatewayClusterInfo { get; } = new CmdletInfo(
            $"{RemoveOnPremisesDataGatewayCluster.CmdletVerb}-{RemoveOnPremisesDataGatewayCluster.CmdletName}",
            typeof(RemoveOnPremisesDataGatewayCluster));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemoveOnPremisesDataGatewayCluster()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(RemoveOnPremisesDataGatewayClusterInfo)
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayCluster.GatewayClusterId), new Guid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.DeleteGatewayCluster(new Guid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
