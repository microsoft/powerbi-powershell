/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
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
    public class NewPowerBIReportTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{NewPowerBIReport.CmdletVerb}-{NewPowerBIReport.CmdletName}", typeof(NewPowerBIReport));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndNewReportIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(Cmdlet)
                    .AddParameter("Path", "./testreport.pbix")
                    .AddParameter("Name", "Test");

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndNewReportWorkspace()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);

                ps.AddCommand(Cmdlet)
                    .AddParameter("Path", "./testreport.pbix")
                    .AddParameter("Name", "Test")
                    .AddParameter("WorkspaceId", workspace.Id.ToString());

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndNewReportsWithoutLogin()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                ps.AddCommand(Cmdlet)
                    .AddParameter("Path", "./testreport.pbix");

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
