/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayClusterStatusTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayClusterStatusInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayClusterStatus.CmdletVerb}-{GetOnPremisesDataGatewayClusterStatus.CmdletName}",
            typeof(GetOnPremisesDataGatewayClusterStatus));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayClusterStatus()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayClusterStatusInfo).AddParameter(nameof(GetOnPremisesDataGatewayClusterStatus.GatewayClusterId), Guid.NewGuid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetOnPremisesDataGatewayClusterStatusReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var clusterStatus = "the cluster status";
            var expectedResponse = new GatewayClusterStatusResponse
            {
                ClusterStatus = clusterStatus,
                GatewayStaticCapabilities = "the static capabilities",
                GatewayVersion = "3000.0.0.0+gabcdef0",
                GatewayUpgradeState = "the upgrade state"
            };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .GetGatewayClusterStatus(gatewayClusterId, true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetOnPremisesDataGatewayClusterStatus(initFactory)
            {
                GatewayClusterId = gatewayClusterId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
