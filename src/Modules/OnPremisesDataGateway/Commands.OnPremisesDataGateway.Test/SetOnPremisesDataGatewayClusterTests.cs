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
    public class SetOnPremisesDataGatewayClusterTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayClusterInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayCluster.CmdletVerb}-{SetOnPremisesDataGatewayCluster.CmdletName}",
            typeof(SetOnPremisesDataGatewayCluster));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayCluster()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayClusterInfo)
                    .AddParameter(nameof(SetOnPremisesDataGatewayCluster.GatewayClusterId), new Guid())
                    .AddParameter(nameof(SetOnPremisesDataGatewayCluster.Name), "name")
                    .AddParameter(nameof(SetOnPremisesDataGatewayCluster.AllowCloudDatasourceRefresh), false);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }
    }
}
