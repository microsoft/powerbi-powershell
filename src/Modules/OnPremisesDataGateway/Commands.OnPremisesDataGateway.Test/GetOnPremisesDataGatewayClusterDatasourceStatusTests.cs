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
    public class GetOnPremisesDataGatewayClusterDatasourceStatusTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayClusterDatasourceStatusInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayClusterDatasourceStatus.CmdletVerb}-{GetOnPremisesDataGatewayClusterDatasourceStatus.CmdletName}",
            typeof(GetOnPremisesDataGatewayClusterDatasourceStatus));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayClusterDatasourceStatus()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayClusterDatasourceStatusInfo)
                    .AddParameter(nameof(GetOnPremisesDataGatewayClusterDatasourceStatus.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(GetOnPremisesDataGatewayClusterDatasourceStatus.GatewayDatasourceId), Guid.NewGuid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetOnPremisesDataGatewayClusterDatasourceStatusReturnsExpectedResults()
        {
            // Arrange
            var expectedResponse = new HttpResponseMessage();
            var gatewayClusterId = Guid.NewGuid();
            var gatewayDatasourceId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .GetGatewayClusterDatasourceStatus(gatewayClusterId, gatewayDatasourceId, true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetOnPremisesDataGatewayClusterDatasourceStatus(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                GatewayDatasourceId = gatewayDatasourceId,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
