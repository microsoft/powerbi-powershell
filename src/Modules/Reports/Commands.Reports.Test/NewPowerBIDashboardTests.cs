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
    public class NewPowerBIDashboardTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{NewPowerBIDashboard.CmdletVerb}-{NewPowerBIDashboard.CmdletName}", typeof(NewPowerBIDashboard));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndNewDashboardIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test Dash: " + Guid.NewGuid().ToString())
                    .AddParameter(nameof(NewPowerBIReport.WorkspaceId), new Guid("0d7cd27b-62f1-494e-9dc8-a7b3e48aab28"));

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndNewDashboardInMyWorkspaceIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test Dash: " + Guid.NewGuid().ToString());

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndNewDashboardWithoutLogin()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test Dash: " + Guid.NewGuid().ToString());

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void NewPowerBIDashboardTest()
        {
            // Arrange 
            var dashboardName = "Mocked Name";
            var expectedResponse = new Dashboard { Id = Guid.NewGuid(), Name = dashboardName };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Reports
                .AddDashboard(dashboardName, Guid.Empty))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new NewPowerBIDashboard(initFactory)
            {
                Name = dashboardName
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
