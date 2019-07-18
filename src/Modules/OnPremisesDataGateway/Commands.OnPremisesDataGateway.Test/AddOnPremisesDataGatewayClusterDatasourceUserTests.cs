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
    public class AddOnPremisesDataGatewayClusterDatasourceUserTests
    {
        private static CmdletInfo AddOnPremisesDataGatewayClusterDatasourceUserInfo { get; } = new CmdletInfo(
            $"{AddOnPremisesDataGatewayClusterDatasourceUser.CmdletVerb}-{AddOnPremisesDataGatewayClusterDatasourceUser.CmdletName}",
            typeof(AddOnPremisesDataGatewayClusterDatasourceUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddOnPremisesDataGatewayClusterDatasourceUser()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(AddOnPremisesDataGatewayClusterDatasourceUserInfo)
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasourceUser.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasourceUser.GatewayClusterDatasourceId), Guid.NewGuid())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasourceUser.DatasourceUserAccessRight), "Read")
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasourceUser.DisplayName), Guid.NewGuid().ToString())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasourceUser.UserEmailAddress), "theEmailAddress@foo.com");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void AddOnPremisesDataGatewayClusterDatasourceUserReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var GatewayClusterDatasourceId = Guid.NewGuid();

            var request = new UserAccessRightEntry()
            {
                DatasourceUserAccessRight = DatasourceUserAccessRight.Read,
                DisplayName = "the user displayName",
                Identifier = Guid.NewGuid().ToString(),
                PrincipalType = PrincipalType.User,
                UserEmailAddress = "theEmailAddress@foo.com",
            };

            var expectedResponse = new HttpResponseMessage();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .AddUsersToGatewayClusterDatasource(gatewayClusterId, GatewayClusterDatasourceId, It.IsAny<UserAccessRightEntry>(), true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddOnPremisesDataGatewayClusterDatasourceUser(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                GatewayClusterDatasourceId = GatewayClusterDatasourceId,
                DatasourceUserAccessRight = request.DatasourceUserAccessRight,
                DisplayName = request.DisplayName,
                UserEmailAddress = request.UserEmailAddress,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(null, client, initFactory);
        }
    }
}
