/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data
{
    [TestClass]
    public class GetPowerBIDataflowTests
    {
        public static CmdletInfo GetPowerBIDataflowCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDataflow.CmdletVerb}-{GetPowerBIDataflow.CmdletName}", typeof(GetPowerBIDataflow));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIDataflowOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIDataflowCmdletInfo).AddParameter(nameof(GetPowerBIDataflow.Scope), PowerBIUserScope.Organization);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void GetPowerBIDataflowIndividualScope_ListParameterSet()
        {
            // Arrange
            var expectedDataflows = new List<Dataflow> { new Dataflow { Id = Guid.NewGuid(), Name = "TestDataflow1" }, new Dataflow { Id = Guid.NewGuid(), Name = "TestDataflow2" } };
            var workspaceId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflows(workspaceId)).Returns(expectedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                ParameterSet = "List",
                WorkspaceId = workspaceId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }

        [TestMethod]
        public void GetPowerBIDataflowOrganizationScope_ListParameterSet()
        {
            // Arrange
            var expectedDataflows = new List<Dataflow> { new Dataflow { Id = Guid.NewGuid(), Name = "TestDataflow1" }, new Dataflow { Id = Guid.NewGuid(), Name = "TestDataflow2" } };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflowsAsAdmin(null, null, null)).Returns(expectedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }

        [TestMethod]
        public void GetPowerBIDataflowOrganizationScope_IdParameterSet()
        {
            // Arrange
            var dataflowId = Guid.NewGuid();
            var expectedDataflows = new List<Dataflow> { new Dataflow { Id = dataflowId, Name = "TestDataflow1" } };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflowsAsAdmin($"id eq '{dataflowId}'", null, null)).Returns(expectedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                ParameterSet = "Id",
                Id = dataflowId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }

        [TestMethod]
        public void GetPowerBIDataflowIndividualScope_IdParameterSet()
        {
            // Arrange
            var firstDataflowId = Guid.NewGuid();
            var firstDataflow = new Dataflow { Id = firstDataflowId, Name = "MyDataflow1" };
            var expectedDataflows = new List<Dataflow> { firstDataflow };
            var returnedDataflows = new List<Dataflow> { firstDataflow, new Dataflow { Id = Guid.NewGuid(), Name = "MyDataflow2" }, new Dataflow { Id = Guid.NewGuid(), Name = "OtherDataflow3" } };
            var workspaceId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflows(workspaceId)).Returns(returnedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                ParameterSet = "Id",
                Id = firstDataflowId,
                WorkspaceId = workspaceId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }

        [TestMethod]
        public void GetPowerBIDataflowOrganizationScope_NameParameterSet()
        {
            // Arrange
            var dataflowName = "TestDataflow1";
            var expectedDataflows = new List<Dataflow> { new Dataflow { Id = Guid.NewGuid(), Name = dataflowName } };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflowsAsAdmin($"tolower(name) eq '{dataflowName.ToLower()}'", null, null)).Returns(expectedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                ParameterSet = "Name",
                Name = dataflowName
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }

        [TestMethod]
        public void GetPowerBIDataflowIndividualScope_NameParameterSet()
        {
            // Arrange
            var firstDataflowName = "TestDataflow1";
            var firstDataflow = new Dataflow { Id = Guid.NewGuid(), Name = firstDataflowName };
            var expectedDataflows = new List<Dataflow> { firstDataflow };
            var returnedDataflows = new List<Dataflow> { firstDataflow, new Dataflow { Id = Guid.NewGuid(), Name = "MyDataflow2" }, new Dataflow { Id = Guid.NewGuid(), Name = "OtherDataflow3" } };
            var workspaceId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflows(workspaceId)).Returns(returnedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                ParameterSet = "Name",
                Name = firstDataflowName,
                WorkspaceId = workspaceId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }
    }
}
