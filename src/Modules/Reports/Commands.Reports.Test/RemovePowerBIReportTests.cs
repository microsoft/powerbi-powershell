/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Commands.Reports;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Commands.Reports.Test
{
    [TestClass]
    public class RemovePowerBIReportTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemovePowerBIReport()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps,PowerBIEnvironmentType.Public); // If login is needed
                ps.AddCommand(new CmdletInfo($"{RemovePowerBIReport.CmdletVerb}-{RemovePowerBIReport.CmdletName}", typeof(RemovePowerBIReport)));
                ps.AddParameter("Id", "fce8abb5-192b-4be2-b75e-b43bb93d8943");
                ps.AddParameter("WorkspaceId", "kjsdfjs;sf");
                var result = ps.Invoke();

                // Add asserts to verify
                
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void RemovePowerBIReportTest()
        {
            // Arrange 
            var reportID = Guid.NewGuid();
            object expectedResponse = null;
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Reports
                .DeleteReport(reportID))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RemovePowerBIReport(initFactory)
            {
                Id = reportID
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
