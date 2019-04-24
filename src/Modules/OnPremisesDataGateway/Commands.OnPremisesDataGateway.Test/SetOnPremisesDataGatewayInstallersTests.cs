/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
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
    public class SetOnPremisesDataGatewayInstallersTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayInstallersInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayInstallers.CmdletVerb}-{SetOnPremisesDataGatewayInstallers.CmdletName}",
            typeof(SetOnPremisesDataGatewayInstallers));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayInstallers()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayInstallersInfo)
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstallers.PrincipalObjectIds), new string[] { new Guid().ToString(), new Guid().ToString()})
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstallers.Operation), OperationType.None)
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstallers.GatewayType), GatewayType.Resource);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task SetOnPremisesDataGatewayInstallersCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayInstallersRequest
            {
                Ids = new string[]
                {
                    "id"
                },
                Operation = OperationType.None,
                GatewayType = GatewayType.Personal
            };

            // Act
            var result = await client.UpdateInstallerPrincipals(request);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }
    }
}
