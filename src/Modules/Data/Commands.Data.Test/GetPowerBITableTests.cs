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
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class GetPowerBITableTests
    {
        private static CmdletInfo GetPowerBITableCmdletInfo => new CmdletInfo($"{GetPowerBITable.CmdletVerb}-{GetPowerBITable.CmdletName}", typeof(GetPowerBITable));
        private static CmdletInfo GetPowerBIDatasetCmdletInfo => new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBITable_DatasetIdParameterSetName()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter(nameof(GetPowerBITable.Scope), PowerBIUserScope.Individual);
                var datasetResults = ps.Invoke();
                ps.Commands.Clear();
                var datasetId = datasetResults.FirstOrDefault(x => (bool)x.Members["AddRowsAPIEnabled"].Value == true).Members["Id"].Value;
                ps.AddCommand(GetPowerBITableCmdletInfo)
                    .AddParameter(nameof(GetPowerBITable.DatasetId), datasetId)
                    .AddParameter(nameof(GetPowerBITable.Name), "Product");
                
                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No tables returned. Verify you have tables under your logged in user.");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBITable_DatasetParameterSetName()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter(nameof(GetPowerBITable.Scope), PowerBIUserScope.Individual);
                ps.AddCommand("Where-Object");
                var filter = ScriptBlock.Create("$_.AddRowsApiEnabled -eq $true");
                ps.AddParameter("FilterScript", filter);
                ps.AddCommand(GetPowerBITableCmdletInfo);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No tables returned. Verify you have tables under your logged in user.");
                }
            }
        }

        [TestMethod]
        public void GetPowerBITable_IndividualScope_DatasetIdParameterSetName()
        {
            var datasetId = Guid.NewGuid();
            var expectedTables = new List<Table> { new Table {  Name = "TestTable" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetTables(datasetId, null)).Returns(expectedTables);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var getTableCmdlet = new GetPowerBITable(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                DatasetId = datasetId
            };

            // Act
            getTableCmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedTables);
        }

        [TestMethod]
        public void GetPowerBITable_OrganizationScope()
        {
            var client = new Mock<IPowerBIApiClient>();
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBITable(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                DatasetId = Guid.NewGuid()
            };

            try
            {
                // Act
                cmdlet.InvokePowerBICmdlet();

                Assert.Fail("Should not have reached this point");
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                // Assert
                Assert.AreEqual(ex.InnerException.GetType(), typeof(NotImplementedException));
            }
        }
    }    
}
