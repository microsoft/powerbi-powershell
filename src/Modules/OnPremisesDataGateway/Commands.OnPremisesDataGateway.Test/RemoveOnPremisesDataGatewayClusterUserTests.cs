/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class RemoveOnPremisesDataGatewayClusterUserTests
    {
        private static CmdletInfo RemoveOnPremisesDataGatewayClusterUserInfo { get; } = new CmdletInfo(
            $"{RemoveOnPremisesDataGatewayClusterUser.CmdletVerb}-{RemoveOnPremisesDataGatewayClusterUser.CmdletName}",
            typeof(RemoveOnPremisesDataGatewayClusterUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemoveOnPremisesDataGatewayClusterUser()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(RemoveOnPremisesDataGatewayClusterUserInfo)
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterUser.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(RemoveOnPremisesDataGatewayClusterUser.PrincipalObjectId), Guid.NewGuid());

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }
    }
}
