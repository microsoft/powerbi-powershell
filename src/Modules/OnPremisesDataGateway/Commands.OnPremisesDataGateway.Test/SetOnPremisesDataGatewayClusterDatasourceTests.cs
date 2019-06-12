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
    public class SetOnPremisesDataGatewayClusterDatasourceTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayClusterDatasourceInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayClusterDatasource.CmdletVerb}-{SetOnPremisesDataGatewayClusterDatasource.CmdletName}",
            typeof(SetOnPremisesDataGatewayClusterDatasource));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayClusterDatasource()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayClusterDatasourceInfo)
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasource.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasource.GatewayDatasourceId), Guid.NewGuid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void SetOnPremisesDataGatewayClusterDatasourceReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var gatewayDatasourceId = Guid.NewGuid();

            var updateRequest = new UpdateGatewayClusterDatasourceRequest
            {
                Annotation = "the annotation",
                DatasourceName = "the datasource name",
                SingleSignOnType = "None",
            };

            var expectedResponse = new HttpResponseMessage();
            var client = new Mock<IPowerBIApiClient>();
            client.SetupSequence(x => x.Gateways
                .UpdateGatewayClusterDatasource(gatewayClusterId, gatewayDatasourceId, It.IsAny<UpdateGatewayClusterDatasourceRequest>(), true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetOnPremisesDataGatewayClusterDatasource(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                GatewayDatasourceId = gatewayDatasourceId,
                Annotation = updateRequest.Annotation,
                DatasourceName = updateRequest.DatasourceName,
                SingleSignOnType = updateRequest.SingleSignOnType,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
