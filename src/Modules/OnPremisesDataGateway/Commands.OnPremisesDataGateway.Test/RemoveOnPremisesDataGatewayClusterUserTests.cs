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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterUser.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterUser.PrincipalObjectId), Guid.NewGuid());

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
            var gatewayClusterId = Guid.NewGuid();
            var principalObjectId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .DeleteUserOnGatewayCluster(gatewayClusterId, principalObjectId, true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RemoveOnPremisesDataGatewayClusterUser(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                PrincipalObjectId = principalObjectId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(null, client, initFactory);
        }
    }
}
