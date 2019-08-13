/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

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
    public class SetOnPremisesDataGatewayTenantPolicyTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayTenantPolicyInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayTenantPolicy.CmdletVerb}-{SetOnPremisesDataGatewayTenantPolicy.CmdletName}",
            typeof(SetOnPremisesDataGatewayTenantPolicy));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayTenantPolicy()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayTenantPolicyInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void SetOnPremisesDataGatewayTenantPolicyReturnsExpectedResults()
        {
            // Arrange
            var expectedResponse = new HttpResponseMessage();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .UpdateTenantPolicy(It.IsAny<UpdateGatewayPolicyRequest>()))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetOnPremisesDataGatewayTenantPolicy(initFactory)
            {
                ResourceGatewayInstallPolicy = PolicyType.Restricted
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(null, client, initFactory);
        }
    }
}
