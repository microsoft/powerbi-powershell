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
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class GetPowerBIArtifactTests
    {
        private static CmdletInfo GetPowerBIArtifactCmdletInfo => new CmdletInfo($"{GetPowerBIArtifact.CmdletVerb}-{GetPowerBIArtifact.CmdletName}", typeof(GetPowerBIArtifact));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIArtifact()
        {
            /*
             * Test requires at least one workspace (group or folder workspace) and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIArtifactCmdletInfo);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
                }
            }
        }

        [TestMethod]
        public void GetPowerBIArtifact_WithValidResponse()
        {
            // Arrange
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client
                .Setup(x => x.Workspaces.GetWorkspacesAsAdmin("reports,dashboards,datasets,dataflows,workbooks", default, 100, default))
                .Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIArtifact(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetPowerBIArtifact_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client
                .Setup(x => x.Workspaces.GetWorkspacesAsAdmin("reports,dashboards,datasets,dataflows,workbooks", default, 100, default))
                .Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIArtifact(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();
        }

        [TestMethod]
        public void GetPowerBIArtifact_WithTopAndSkipPresent()
        {
            // Arrange
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client
                .Setup(x => x.Workspaces.GetWorkspacesAsAdmin("reports,dashboards,datasets,dataflows,workbooks", default, 101, 1))
                .Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIArtifact(initFactory)
            {
                First = 101,
                Skip = 1,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetPowerBIArtifact_WithAllPresent()
        {
            // Arrange
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client
                .Setup(x => x.Workspaces.GetWorkspacesAsAdmin("reports,dashboards,datasets,dataflows,workbooks", default, 5000, 0))
                .Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIArtifact(initFactory)
            {
                All = true,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        private static void AssertExpectedUnitTestResults(List<Workspace> expectedWorkspaces, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.AreEqual(expectedWorkspaces.Count, results.Count());
            var workspaces = results.Cast<Workspace>().ToList();
            CollectionAssert.AreEqual(expectedWorkspaces, workspaces);
        }
    }
}
