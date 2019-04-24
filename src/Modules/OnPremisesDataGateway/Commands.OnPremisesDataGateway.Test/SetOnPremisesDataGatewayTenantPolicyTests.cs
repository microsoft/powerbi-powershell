/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public async Task SetOnPremisesDataGatewayTenantPolicyCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayPolicyRequest()
            {
                ResourceGatewayInstallPolicy = PolicyType.Restricted,
                PersonalGatewayInstallPolicy = PolicyType.None
            };

            // Act
            var result = await client.UpdateTenantPolicy(request);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
