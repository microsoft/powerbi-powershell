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
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class GetPowerBIDatasourceTests
    {
        public static CmdletInfo GetPowerBIDatasourceCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDatasource.CmdletVerb}-{GetPowerBIDatasource.CmdletName}", typeof(GetPowerBIDatasource));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIDatasourceIndividualScope()
        {
            /*
             * Requirement to run test:
             * Need at least one dataset containing a datasource assigned to the user logging into the test.
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIDatasetTests.GetPowerBIDatasetCmdletInfo);
                var existingDatasets = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                ps.Commands.Clear();

                if(!existingDatasets.Any())
                {
                    Assert.Inconclusive("No datasets returned. Verify you have datasets under your logged in user.");
                }

                var testDataset = existingDatasets.Select(d => (Dataset)d.BaseObject).FirstOrDefault();
                ps.AddCommand(GetPowerBIDatasourceCmdletInfo).AddParameter(nameof(GetPowerBIDatasource.DatasetId), testDataset.Id.ToString());

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
        public void EndToEndGetPowerBIDatasourceWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIDatasource.DatasetId), Guid.NewGuid() }
                };
                ps.AddCommand(GetPowerBIDatasourceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndGetPowerBIDatasourceOrganizationScopeAndWorkspaceId()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIDatasource.DatasetId), Guid.NewGuid() },
                    { nameof(GetPowerBIDatasource.WorkspaceId), Guid.NewGuid() },
                    { nameof(GetPowerBIDatasource.Scope), PowerBIUserScope.Organization }
                };
                ps.AddCommand(GetPowerBIDatasourceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndPipingDatasetIntoGetPowerBIDatasourceIndividualScope()
        {
            /*
             * Requirement to run test:
             * Need at least one dataset containing a datasource assigned to the user logging into the test.
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetTests.GetPowerBIDatasetCmdletInfo).AddCommand(GetPowerBIDatasourceCmdletInfo);

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
        public void GetPowerBIDatasourceIndividualScope_ListParameterSet()
        {
            // Arrange
            var datasetId = Guid.NewGuid();
            var expectedDatasources = new List<Datasource>
            {
                new Datasource
                {
                    DatasourceId = Guid.NewGuid(),
                    Name = "TestDatasource",
                    GatewayId = Guid.NewGuid()
                }
            };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetDatasources(datasetId, null)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                DatasetId = datasetId,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }

        [TestMethod]
        public void GetPowerBIDatasourceIndividualScopeAndWorkspaceId_ListParameterSet()
        {
            // Arrange
            var datasetId = Guid.NewGuid();
            var workspaceId = Guid.NewGuid();
            var expectedDatasources = new List<Datasource>
            {
                new Datasource
                {
                    DatasourceId = Guid.NewGuid(),
                    Name = "TestDatasource",
                    GatewayId = Guid.NewGuid()
                }
            };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetDatasources(datasetId, workspaceId)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                DatasetId = datasetId,
                WorkspaceId = workspaceId,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }

        [TestMethod]
        public void GetPowerBIDatasourceOrganizationScope_ListParameterSet()
        {
            // Arrange
            var datasetId = Guid.NewGuid();
            var expectedDatasources = new List<Datasource>
            {
                new Datasource
                {
                    DatasourceId = Guid.NewGuid(),
                    Name = "TestDatasource",
                    GatewayId = Guid.NewGuid()
                }
            };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetDatasourcesAsAdmin(datasetId)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                DatasetId = datasetId,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }

        [TestMethod]
        public void GetPowerBIDatasourceIndividualScope_ObjectAndListParameterSet()
        {
            // Arrange
            var testDataset = new Dataset { Id = Guid.NewGuid(), Name = "TestDataset" };
            var expectedDatasources = new List<Datasource>
            {
                new Datasource
                {
                    DatasourceId = Guid.NewGuid(),
                    Name = "TestDatasource",
                    GatewayId = Guid.NewGuid()
                }
            };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetDatasources(testDataset.Id, null)).Returns(expectedDatasources);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDatasource(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Dataset = testDataset,
                ParameterSet = "ObjectAndList",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasources);
        }
    }
}
