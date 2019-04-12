using System;
using System.Management.Automation;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class AddOnPremisesDataGatewayClusterUserTests
    {
        public static CmdletInfo AddOnPremisesDataGatewayClusterUserInfo { get; } = new CmdletInfo(
            $"{AddOnPremisesDataGatewayClusterUser.CmdletVerb}-{AddOnPremisesDataGatewayClusterUser.CmdletName}",
            typeof(AddOnPremisesDataGatewayClusterUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddOnPremisesDataGatewayClusterUser()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(AddOnPremisesDataGatewayClusterUserInfo)
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.GatewayClusterId), new Guid())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.PrincipalObjectId), new Guid("{F31FEA72-8435-4871-BF75-E94168C71A6D}"))
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.AllowedDataSourceTypes), new DatasourceType[] {DatasourceType.Sql })
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.Role), "ConnectionCreator");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

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
