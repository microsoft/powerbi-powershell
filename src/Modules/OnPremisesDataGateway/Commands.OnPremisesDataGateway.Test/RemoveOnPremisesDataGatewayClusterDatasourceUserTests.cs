/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
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
    public class RemoveOnPremisesDataGatewayClusterDatasourceUserTests
    {
        private static CmdletInfo RemoveOnPremisesDataGatewayClusterDatasourceUserInfo { get; } = new CmdletInfo(
            $"{RemoveOnPremisesDataGatewayClusterDatasourceUser.CmdletVerb}-{RemoveOnPremisesDataGatewayClusterDatasourceUser.CmdletName}",
            typeof(RemoveOnPremisesDataGatewayClusterDatasourceUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemoveOnPremisesDataGatewayClusterDatasourceUser()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(RemoveOnPremisesDataGatewayClusterDatasourceUserInfo)
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterDatasourceUser.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterDatasourceUser.GatewayClusterDatasourceId), Guid.NewGuid())
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterDatasourceUser.UserId), "theEmailAddress@foo.com");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void RemoveOnPremisesDataGatewayClusterDatasourceUserReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var GatewayClusterDatasourceId = Guid.NewGuid();
            var emailAddress = Guid.NewGuid().ToString();

            var expectedResponse = new HttpResponseMessage();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .RemoveGatewayClusterDatasourceUser(gatewayClusterId, GatewayClusterDatasourceId, emailAddress, true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RemoveOnPremisesDataGatewayClusterDatasourceUser(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                GatewayClusterDatasourceId = GatewayClusterDatasourceId,
                UserId = emailAddress,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
