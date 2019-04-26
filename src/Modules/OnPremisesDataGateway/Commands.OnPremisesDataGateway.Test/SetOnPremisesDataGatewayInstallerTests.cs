/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
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
    public class SetOnPremisesDataGatewayInstallerTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayInstallerInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayInstaller.CmdletVerb}-{SetOnPremisesDataGatewayInstaller.CmdletName}",
            typeof(SetOnPremisesDataGatewayInstaller));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayInstaller()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayInstallerInfo)
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstaller.PrincipalObjectIds), new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString()})
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstaller.Operation), OperationType.None)
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstaller.GatewayType), GatewayType.Resource);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void SetOnPremisesDataGatewayInstallerReturnsExpectedResults()
        {
            // Arrange
            var expectedResponse = new HttpResponseMessage();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .UpdateInstallerPrincipals(It.IsAny<UpdateGatewayInstallersRequest>()))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetOnPremisesDataGatewayInstaller(initFactory)
            {
                PrincipalObjectIds = new string[]
                {
                    "id"
                },
                Operation = OperationType.None,
                GatewayType = GatewayType.Personal
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
