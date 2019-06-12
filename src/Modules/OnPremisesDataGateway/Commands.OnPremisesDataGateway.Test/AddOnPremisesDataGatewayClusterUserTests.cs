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
    public class AddOnPremisesDataGatewayClusterUserTests
    {
        private static CmdletInfo AddOnPremisesDataGatewayClusterUserInfo { get; } = new CmdletInfo(
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
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.PrincipalObjectId), Guid.NewGuid())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.AllowedDataSourceTypes), new DatasourceType[] { DatasourceType.Sql })
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterUser.Role), "ConnectionCreator");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void AddOnPremisesDataGatewayClusterUserReturnsExpectedResults()
        {
            // Arrange
            var allowedDatasourceTypes = new List<DatasourceType>()
            {
                DatasourceType.Sql
            };
            var gatewayClusterId = Guid.NewGuid();
            var request = new GatewayClusterAddPrincipalRequest()
            {
                PrincipalObjectId = Guid.NewGuid(),
                AllowedDataSourceTypes = allowedDatasourceTypes,
                Role = "the role"

            };

            var expectedResponse = new HttpResponseMessage();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .AddUsersToGatewayCluster(gatewayClusterId, It.IsAny<GatewayClusterAddPrincipalRequest>(), true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddOnPremisesDataGatewayClusterUser(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                PrincipalObjectId = request.PrincipalObjectId,
                AllowedDataSourceTypes = allowedDatasourceTypes,
                Role = request.Role
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
