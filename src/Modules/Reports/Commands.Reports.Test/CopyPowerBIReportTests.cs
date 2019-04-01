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
    public class CopyPowerBIReportTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{CopyPowerBIReport.CmdletVerb}-{CopyPowerBIReport.CmdletName}", typeof(CopyPowerBIReport));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndCopyReportIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(CopyPowerBIReport.Name), "Copied Report: " + Guid.NewGuid().ToString())
                    .AddParameter(nameof(CopyPowerBIReport.Id), new Guid("30ca8f24-f628-45f7-a5ac-540c95e9b5e6"));

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndCopyReportToMyWorkspaceIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(CopyPowerBIReport.Name), "Copied Report: " + Guid.NewGuid().ToString())
                    .AddParameter(nameof(CopyPowerBIReport.Id), new Guid("d46f65e7-b86a-4ea6-a636-714bcf06ab88"))
                    .AddParameter(nameof(CopyPowerBIReport.WorkspaceId), new Guid("0d7cd27b-62f1-494e-9dc8-a7b3e48aab28"))
                    .AddParameter(nameof(CopyPowerBIReport.TargetWorkspaceId), Guid.Empty)
                    .AddParameter(nameof(CopyPowerBIReport.TargetDatasetId), new Guid("1b46e4dc-1299-425b-97aa-c10d51f82a06"));

                // Act
                var reportId = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndCopyReportWithoutLogin()
        {
            using (var ps = PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                ps.AddCommand(Cmdlet)
                    .AddParameter(nameof(CopyPowerBIReport.Name), "Copied Report: " + Guid.NewGuid().ToString())
                    .AddParameter(nameof(CopyPowerBIReport.Id), new Guid("d46f65e7-b86a-4ea6-a636-714bcf06ab88"))
                    .AddParameter(nameof(CopyPowerBIReport.WorkspaceId), new Guid("0d7cd27b-62f1-494e-9dc8-a7b3e48aab28"))
                    .AddParameter(nameof(CopyPowerBIReport.TargetWorkspaceId), Guid.Empty)
                    .AddParameter(nameof(CopyPowerBIReport.TargetDatasetId), new Guid("1b46e4dc-1299-425b-97aa-c10d51f82a06"));

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void CopyPowerBIReportTest()
        {
            // Arrange 
            var reportName = "Mocked Name";
            var reportId = Guid.NewGuid();
            var expectedResponse = new Report();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Reports
                .CopyReport(reportName, Guid.Empty.ToString(), reportId.ToString(), Guid.Empty.ToString(), Guid.Empty.ToString()))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new CopyPowerBIReport(initFactory)
            {
                Name = reportName,
                Id = reportId,
                WorkspaceId = Guid.Empty.ToString(),
                TargetWorkspaceId = Guid.Empty.ToString(),
                TargetDatasetId = Guid.Empty.ToString()
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void CopyPowerBIReportObjectTest()
        {
            // Arrange 
            var reportName = "Mocked Name";
            var reportId = Guid.NewGuid();
            var report = new Report { Id = reportId, Name = reportName };
            var expectedResponse = new Report();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Reports
                .CopyReport(reportName, Guid.Empty.ToString(), reportId.ToString(), Guid.Empty.ToString(), Guid.Empty.ToString()))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new CopyPowerBIReport(initFactory)
            {
                Report = report,
                WorkspaceId = Guid.Empty.ToString(),
                TargetWorkspaceId = Guid.Empty.ToString(),
                TargetDatasetId = Guid.Empty.ToString()
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
