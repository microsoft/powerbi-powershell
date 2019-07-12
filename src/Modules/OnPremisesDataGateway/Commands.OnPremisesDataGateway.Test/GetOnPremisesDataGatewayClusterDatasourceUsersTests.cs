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
    public class GetOnPremisesDataGatewayClusterDatasourceUsersTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayClusterDatasourceUsersInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayClusterDatasourceUsers.CmdletVerb}-{GetOnPremisesDataGatewayClusterDatasourceUsers.CmdletName}",
            typeof(GetOnPremisesDataGatewayClusterDatasourceUsers));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayClusterDatasourceUsers()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayClusterDatasourceUsersInfo)
                    .AddParameter(nameof(GetOnPremisesDataGatewayClusterDatasourceUsers.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(GetOnPremisesDataGatewayClusterDatasourceUsers.GatewayClusterDatasourceId), Guid.NewGuid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetOnPremisesDataGatewayClusterDatasourceUsersReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var GatewayClusterDatasourceId = Guid.NewGuid();

            var expectedResponse = new DatasourceUser
            {
                DatasourceUserAccessRight = "ReadWrite",
                DisplayName = "the user displayName1",
                Identifier = Guid.NewGuid().ToString(),
                PrincipalType = "User",
                EmailAddress = "theEmailAddress1@foo.com",
            };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .GetGatewayClusterDatasourceUsers(gatewayClusterId, GatewayClusterDatasourceId, true))
                .ReturnsAsync(new[] { expectedResponse });

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetOnPremisesDataGatewayClusterDatasourceUsers(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                GatewayClusterDatasourceId = GatewayClusterDatasourceId,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
