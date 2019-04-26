/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
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
            $"{SetOnPremisesDataGatewayInstaller.CmdletVerb}-{SetOnPremisesDataGatewayInstaller.CmdletName}",
            typeof(SetOnPremisesDataGatewayInstaller));

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
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstaller.PrincipalObjectIds), new string[] { new Guid().ToString(), new Guid().ToString()})
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstaller.Operation), OperationType.None)
                    .AddParameter(nameof(SetOnPremisesDataGatewayInstaller.GatewayType), GatewayType.Resource);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }
    }
}
