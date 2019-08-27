/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.PowerBI.Common.Api.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class GetPowerBIDataflowDatasourceTests
    {
        public static CmdletInfo GetPowerBIDataflowDatasourceCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDataflowDatasource.CmdletVerb}-{GetPowerBIDataflowDatasource.CmdletName}", typeof(GetPowerBIDataflowDatasource));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIDataflowDatasourceOrganizationScope()
        {
            /*
             * Requirement to run test:
             * Need at least one dataflow containing a datasource assigned to the user logging into the test.
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIDataflowTests.GetPowerBIDataflowCmdletInfo).AddParameter(nameof(GetPowerBIDataflowDatasource.Scope), PowerBIUserScope.Organization);

                var existingDataflows = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                ps.Commands.Clear();

                if(!existingDataflows.Any())
                {
                    Assert.Inconclusive("No dataflows returned. Verify you have dataflows under your logged in user.");
                }

                var testDataflow = existingDataflows.Select(d => (Dataflow)d.BaseObject).FirstOrDefault();

                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIDataflowDatasource.DataflowId), testDataflow.Id },
                    { nameof(GetPowerBIDataflowDatasource.Scope), PowerBIUserScope.Organization }
                };

                ps.AddCommand(GetPowerBIDataflowDatasourceCmdletInfo).AddParameters(parameters);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndGetPowerBIDataflowDatasourceWithoutLoginFail()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIDataflowDatasource.DataflowId), Guid.NewGuid() },
                    { nameof(GetPowerBIDataflowDatasource.WorkspaceId), Guid.NewGuid() }
                };
                ps.AddCommand(GetPowerBIDataflowDatasourceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndGetPowerBIDataflowDatasourceOrganizationScopeWithoutLoginFail()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange - 
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIDataflowDatasource.DataflowId), Guid.NewGuid() },
                    { nameof(GetPowerBIDataflowDatasource.Scope), PowerBIUserScope.Organization }
                };
                ps.AddCommand(GetPowerBIDataflowDatasourceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndPipingDataflowIntoGetPowerBIDataflowDatasourceOrganizationScope()
        {
            /*
             * Requirement to run test:
             * Need at least one dataflow containing a datasource assigned to the user logging into the test.
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);

                ps.AddCommand(GetPowerBIDataflowTests.GetPowerBIDataflowCmdletInfo).AddParameter(nameof(GetPowerBIDataflowDatasource.Scope), PowerBIUserScope.Organization)
                    .AddCommand(GetPowerBIDataflowDatasourceCmdletInfo).AddParameter(nameof(GetPowerBIDataflowDatasource.Scope), PowerBIUserScope.Organization);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No datasources returned. Verify you have datasources under your logged in user.");
                }

                Assert.IsTrue(results.Count > 0);
            }
        }

        [TestMethod]
        public void GetPowerBIDataflowDatasourceIndividualScopeAndWorkspaceId_ListParameterSet()
        {
            // Arrange
            var dataflowId = Guid.NewGuid();
            var workspaceId = Guid.NewGuid();
            var expectedDatasources = new List<Datasource> { new Datasource { DatasourceId = Guid.NewGuid().ToString(), Name = "TestDatasource", GatewayId = Guid.NewGuid().ToString() } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflowDatasources(workspaceId, dataflowId)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflowDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                DataflowId = dataflowId,
                WorkspaceId = workspaceId,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }

        [TestMethod]
        public void GetPowerBIDataflowDatasourceOrganizationScope_ListParameterSet()
        {
            // Arrange
            var dataflowId = Guid.NewGuid();
            var expectedDatasources = new List<Datasource> { new Datasource { DatasourceId = Guid.NewGuid().ToString(), Name = "TestDatasource", GatewayId = Guid.NewGuid().ToString() } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflowDatasourcesAsAdmin(dataflowId)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflowDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                DataflowId = dataflowId,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }

        [TestMethod]
        public void GetPowerBIDataflowDatasourceIndividualScope_ObjectAndListParameterSet()
        {
            // Arrange
            var testDataflow = new Dataflow { Id = Guid.NewGuid(), Name = "TestDataflow" };
            var expectedDatasources = new List<Datasource> { new Datasource { DatasourceId = Guid.NewGuid().ToString(), Name = "TestDatasource", GatewayId = Guid.NewGuid().ToString() } };
            var workspaceId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflowDatasources(workspaceId, testDataflow.Id)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflowDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Dataflow = testDataflow,
                WorkspaceId = workspaceId,
                ParameterSet = "DataflowAndList",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }
    }
}
