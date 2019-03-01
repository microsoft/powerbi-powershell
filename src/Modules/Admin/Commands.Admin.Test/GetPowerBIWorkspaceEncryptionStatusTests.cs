/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class GetPowerBIWorkspaceEncryptionStatusTests
    {
        private static CmdletInfo GetPowerBIWorkspaceEncryptionStatusCmdletInfo => new CmdletInfo($"{GetPowerBIWorkspaceEncryptionStatus.CmdletVerb}-{GetPowerBIWorkspaceEncryptionStatus.CmdletName}", typeof(GetPowerBIWorkspaceEncryptionStatus));

        private static string MockName = "GroupName";

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIWorkspaceEncryptionStatus()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIWorkspaceEncryptionStatusCmdletInfo)
                        .AddParameter(nameof(GetPowerBIWorkspaceEncryptionStatus.Name), MockName);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void GetPowerBIWorkspaceEncryptionStatus_WithNameParameterSet()
        {
            // Arrange
            var workspace1 = new Workspace { Id = Guid.NewGuid(), Name = "Workspace1" };
            var workspaces = new List<Workspace>();
            workspaces.Add(workspace1);
            var datasetEncryptionStatus1 = new Dataset() {
                Id = "Dataset1",
                Encryption = new Encryption { EncryptionStatus = EncryptionStatus.InSyncWithWorkspace }
            };
            var datasetEncryptionStatus2 = new Dataset()
            {
                Id = "Dataset2",
                Encryption = new Encryption { EncryptionStatus = EncryptionStatus.NotInSyncWithWorkspace }
            };
            var datasetEncryptionStatus = new List<Dataset>();
            datasetEncryptionStatus.Add(datasetEncryptionStatus1);
            datasetEncryptionStatus.Add(datasetEncryptionStatus2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(default, "name eq 'Workspace1'", 1, default)).Returns(workspaces);
            client.Setup(x => x.Admin.GetPowerBIWorkspaceEncryptionStatus(It.IsAny<string>())).Returns(datasetEncryptionStatus);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspaceEncryptionStatus(initFactory)
            {
                Name = "Workspace1",
                ParameterSet = GetPowerBIWorkspaceEncryptionStatus.NameParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Workspaces.GetWorkspacesAsAdmin(default, "name eq 'Workspace1'", 1, default), Times.Once());
            client.Verify(x => x.Admin.GetPowerBIWorkspaceEncryptionStatus(workspace1.Id.ToString()), Times.Once());
            AssertExpectedUnitTestResults(datasetEncryptionStatus, initFactory);
        }

        [TestMethod]
        public void GetPowerBIWorkspaceEncryptionStatus_WithIdParameterSet()
        {
            // Arrange
            var datasetEncryptionStatus1 = new Dataset()
            {
                Id = "Dataset1",
                Encryption = new Encryption { EncryptionStatus = EncryptionStatus.InSyncWithWorkspace }
            };
            var datasetEncryptionStatus2 = new Dataset()
            {
                Id = "Dataset2",
                Encryption = new Encryption { EncryptionStatus = EncryptionStatus.NotInSyncWithWorkspace }
            };
            var datasetEncryptionStatus = new List<Dataset>();
            datasetEncryptionStatus.Add(datasetEncryptionStatus1);
            datasetEncryptionStatus.Add(datasetEncryptionStatus2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIWorkspaceEncryptionStatus(It.IsAny<string>())).Returns(datasetEncryptionStatus);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var workspaceId = Guid.NewGuid();
            var cmdlet = new GetPowerBIWorkspaceEncryptionStatus(initFactory)
            {
                Id = workspaceId,
                ParameterSet = GetPowerBIWorkspaceEncryptionStatus.IdParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIWorkspaceEncryptionStatus(workspaceId.ToString()), Times.Once());
            AssertExpectedUnitTestResults(datasetEncryptionStatus, initFactory);
        }

        [TestMethod]
        public void GetPowerBIWorkspaceEncryptionStatus_WithWorkspaceParameterSet()
        {
            // Arrange
            var datasetEncryptionStatus1 = new Dataset()
            {
                Id = "Dataset1",
                Encryption = new Encryption { EncryptionStatus = EncryptionStatus.InSyncWithWorkspace }
            };
            var datasetEncryptionStatus2 = new Dataset()
            {
                Id = "Dataset2",
                Encryption = new Encryption { EncryptionStatus = EncryptionStatus.NotInSyncWithWorkspace }
            };
            var datasetEncryptionStatus = new List<Dataset>();
            datasetEncryptionStatus.Add(datasetEncryptionStatus1);
            datasetEncryptionStatus.Add(datasetEncryptionStatus2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIWorkspaceEncryptionStatus(It.IsAny<string>())).Returns(datasetEncryptionStatus);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var workspace = new Workspace { Id = Guid.NewGuid() };
            var cmdlet = new GetPowerBIWorkspaceEncryptionStatus(initFactory)
            {
                Workspace = workspace,
                ParameterSet = GetPowerBIWorkspaceEncryptionStatus.WorkspaceParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIWorkspaceEncryptionStatus(workspace.Id.ToString()), Times.Once());
            AssertExpectedUnitTestResults(datasetEncryptionStatus, initFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetPowerBIWorkspaceEncryptionStatus_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(default, $"name eq '{MockName}'", 1, default)).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspaceEncryptionStatus(initFactory)
            {
                Name = MockName,
                ParameterSet = GetPowerBIWorkspaceEncryptionStatus.NameParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();
        }

        [TestMethod]
        public void GetPowerBIWorkspaceEncryptionStatus_WithNoMatchingWorkspaces()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(default, "name eq 'Workspace1'", 1, default)).Returns(new List<Workspace>());
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspaceEncryptionStatus(initFactory)
            {
                Name = "Workspace1",
                ParameterSet = GetPowerBIWorkspaceEncryptionStatus.NameParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw Exception");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "No matching workspace was found");
        }

        private static void AssertExpectedUnitTestResults(IEnumerable<Dataset> datasetEncryptionStatuses, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.AreEqual(results.Count, 2);
            var datasetEncryptionStatusesList = datasetEncryptionStatuses.ToList();
            Assert.AreEqual(datasetEncryptionStatusesList[0], results[0]);
            Assert.AreEqual(datasetEncryptionStatusesList[1], results[1]);
        }
    }
}
