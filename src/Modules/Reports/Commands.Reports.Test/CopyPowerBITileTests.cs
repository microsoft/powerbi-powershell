/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Commands.Reports;
using Microsoft.PowerBI.Commands.Workspaces.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Commands.Reports.Test
{
    [TestClass]
    public class CopyPowerBITileTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{CopyPowerBITile.CmdletVerb}-{CopyPowerBITile.CmdletName}", typeof(CopyPowerBITile));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndCopyTileIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(CopyPowerBITile.DashboardId), "cff24b2e-faa8-4683-8ecb-2c50e7d2cc7a")
                    .AddParameter(nameof(CopyPowerBITile.TileId), "e297e105-be30-4482-8531-152cdf289ac6")
                    .AddParameter(nameof(CopyPowerBITile.TargetDashboardId), "3a88dd8b-6562-45ab-85e6-85498fd1b4a3")
                    .AddParameter(nameof(CopyPowerBITile.TargetWorkspaceId), "0d7cd27b-62f1-494e-9dc8-a7b3e48aab28")
                    .AddParameter(nameof(CopyPowerBITile.TargetReportId), "d46f65e7-b86a-4ea6-a636-714bcf06ab88")
                    .AddParameter(nameof(CopyPowerBITile.TargetDatasetId), "499adf18-6d12-481b-bf6d-6e0b69324a63");

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndCopyTileWithoutLogin()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(CopyPowerBITile.DashboardId), "cff24b2e-faa8-4683-8ecb-2c50e7d2cc7a")
                    .AddParameter(nameof(CopyPowerBITile.TileId), "e297e105-be30-4482-8531-152cdf289ac6")
                    .AddParameter(nameof(CopyPowerBITile.TargetDashboardId), "3a88dd8b-6562-45ab-85e6-85498fd1b4a3")
                    .AddParameter(nameof(CopyPowerBITile.TargetWorkspaceId), "0d7cd27b-62f1-494e-9dc8-a7b3e48aab28")
                    .AddParameter(nameof(CopyPowerBITile.TargetReportId), "d46f65e7-b86a-4ea6-a636-714bcf06ab88")
                    .AddParameter(nameof(CopyPowerBITile.TargetDatasetId), "499adf18-6d12-481b-bf6d-6e0b69324a63");

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void CopyPowerBITileTest()
        {
            // Arrange 
            var dashboardId = Guid.NewGuid().ToString();
            var targetDashboardId = Guid.NewGuid().ToString();
            var tileId = Guid.NewGuid().ToString();
            var expectedResponse = new Tile();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Reports
                .CopyTile(Guid.Empty, dashboardId, tileId, targetDashboardId, null, null, null, null))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new CopyPowerBITile(initFactory)
            {
                WorkspaceId = Guid.Empty,
                DashboardId = dashboardId,
                TileId = tileId,
                TargetDashboardId = targetDashboardId,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
