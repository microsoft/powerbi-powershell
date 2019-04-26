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
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayCluster.GatewayClusterId), Guid.NewGuid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void RemoveOnPremisesDataGatewayClusterReturnsExpectedResults()
        {
            // Arrange
            var expectedResponse = new HttpResponseMessage();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .DeleteGatewayCluster(It.IsAny<Guid>(), true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RemoveOnPremisesDataGatewayCluster(initFactory)
            {
                GatewayClusterId = Guid.NewGuid(),
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
