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
    public class GetOnPremisesDataGatewayTenantPolicyTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayTenantPolicyInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayTenantPolicy.CmdletVerb}-{GetOnPremisesDataGatewayTenantPolicy.CmdletName}",
            typeof(GetOnPremisesDataGatewayTenantPolicy));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayTenantPolicy()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayTenantPolicyInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetOnPremisesDataGatewayTenantPolicyReturnsExpectedResults()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();
            var expectedResponse = new GatewayTenant
            {
                TenantObjectId = tenantId,
                Policy = TenantPolicy.None
            };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .GetTenantPolicy())
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetOnPremisesDataGatewayTenantPolicy(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
