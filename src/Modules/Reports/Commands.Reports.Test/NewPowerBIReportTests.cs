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
using Microsoft.PowerBI.Commands.Workspaces;
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
        private static CmdletInfo CopyCmdlet => new CmdletInfo($"{CopyPowerBIReport.CmdletVerb}-{CopyPowerBIReport.CmdletName}", typeof(CopyPowerBIReport));
        private static CmdletInfo NewWSCmdlet => new CmdletInfo($"{NewPowerBIWorkspace.CmdletVerb}-{NewPowerBIWorkspace.CmdletName}", typeof(NewPowerBIWorkspace));


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
                    .AddParameter(nameof(NewPowerBIReport.Path), "./testreport.pbix")
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test");

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
                    .AddParameter(nameof(NewPowerBIReport.Path), "./testreport.pbix")
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test")
                    .AddParameter(nameof(NewPowerBIReport.WorkspaceId), workspace.Id.ToString());

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndNewReportWorkspaceWithoutName()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);

                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(NewPowerBIReport.Path), "./testreport.pbix")
                    .AddParameter(nameof(NewPowerBIReport.WorkspaceId), workspace.Id.ToString());

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndNewReportWorkspaceWithMultipleReports()
        {
            Guid expectedReportId;
            string workSpaceId;
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);

                // Create New Workspace
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(NewPowerBIWorkspace.Name), "Test Workspace" + Guid.NewGuid().ToString()},
                };
                ps.AddCommand(NewWSCmdlet).AddParameters(parameters);
                var results = ps.Invoke();
                Dictionary<string, object> serializedObject = null;
                foreach (var r in results)
                {
                    serializedObject = r.Properties.ToDictionary(k => k.Name, v => v.Value);
                }
                workSpaceId = serializedObject["Id"].ToString();

                TestUtilities.AssertNoCmdletErrors(ps);
                ps.Commands.Clear();

                // Create New Report
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(NewPowerBIReport.Path), "./testreport.pbix")
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test")
                    .AddParameter(nameof(NewPowerBIReport.WorkspaceId), workSpaceId);

                var reports = ps.Invoke();
                foreach (var r in reports)
                {
                    serializedObject = r.Properties.ToDictionary(k => k.Name, v => v.Value);
                }
                expectedReportId = (Guid)serializedObject["Id"];
                TestUtilities.AssertNoCmdletErrors(ps);
                ps.Commands.Clear();

                // Copy Report
                ps.AddCommand(CopyCmdlet)
                    .AddParameter(nameof(CopyPowerBIReport.Name), "Copied Report: " + Guid.NewGuid().ToString())
                    .AddParameter(nameof(CopyPowerBIReport.Id), expectedReportId)
                    .AddParameter(nameof(CopyPowerBIReport.WorkspaceId), workSpaceId);

                var report = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                ps.Commands.Clear();

                // update Report (Dataset)
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(NewPowerBIReport.Path), "./testreport.pbix")
                    .AddParameter(nameof(NewPowerBIReport.Name), "Test2.pbix")
                    .AddParameter(nameof(NewPowerBIReport.WorkspaceId), workSpaceId)
                    .AddParameter(nameof(NewPowerBIReport.ConflictAction), ImportConflictHandlerModeEnum.CreateOrOverwrite);

                // Act
                reports = ps.Invoke();

                foreach (var r in reports)
                {
                    serializedObject = r.Properties.ToDictionary(k => k.Name, v => v.Value);
                }

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.AreNotEqual(expectedReportId, (Guid)serializedObject["Id"]);
                Assert.AreEqual("Test2", serializedObject["Name"]);
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
                    .AddParameter(nameof(NewPowerBIReport.Path), "./testreport.pbix");

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
