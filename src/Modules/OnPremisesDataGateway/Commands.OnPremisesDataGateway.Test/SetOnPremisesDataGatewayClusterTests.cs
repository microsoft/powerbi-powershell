/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using System.Net.Http;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class SetOnPremisesDataGatewayClusterTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayClusterInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayCluster.CmdletVerb}-{SetOnPremisesDataGatewayCluster.CmdletName}",
            typeof(SetOnPremisesDataGatewayCluster));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayCluster()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayClusterInfo)
                    .AddParameter(nameof(SetOnPremisesDataGatewayCluster.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(SetOnPremisesDataGatewayCluster.Name), "name")
                    .AddParameter(nameof(SetOnPremisesDataGatewayCluster.AllowCloudDatasourceRefresh), false);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void SetOnPremisesDataGatewayClusterReturnsExpectedResults()
        {
            // Arrange
            var expectedResponse = new HttpResponseMessage();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .PatchGatewayCluster(It.IsAny<Guid>(), It.IsAny<PatchGatewayClusterRequest>(), true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetOnPremisesDataGatewayCluster(initFactory)
            {
                GatewayClusterId = Guid.NewGuid(),
                Name = "name",
                Department = "department",
                Description = "description",
                ContactInformation = "contactInformation",
                AllowCloudDatasourceRefresh = true,
                AllowCustomConnectors = true,
                LoadBalancingSelectorType = "loadBalancingSelectorType"
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
