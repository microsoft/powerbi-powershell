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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter("Scope", PowerBIUserScope.Individual);
                var dataSetResult = ps.Invoke();
                ps.Commands.Clear();
                ps.AddCommand(GetPowerBITableCmdletInfo)
                    .AddParameter("DatasetId", dataSetResult.Where(x => (bool)x.Members["addRowsAPIEnabled"].Value == true).FirstOrDefault().Members["Id"].Value)
                    .AddParameter("Name", "Product");
                
                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
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
                ProfileTestUtilities.ConnectToPowerBI(ps, "Public");
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter("Scope", PowerBIUserScope.Individual);
                ps.AddCommand("Where-Object");
                var filter = ScriptBlock.Create("$_.AddRowsApiEnabled -eq $true");
                ps.AddParameter("FilterScript", filter);
                ps.AddCommand(GetPowerBITableCmdletInfo);

                // Act
                var result = ps.Invoke();
                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void GetPowerBITable_DatasetIdParameterSetName()
        {
            var expectedDatasets = new List<Dataset> { new Dataset { Id = Guid.NewGuid(), Name = "TestDataset" } };
            var expectedTables = new List<Table> { new Table {  Name = "TestTable" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetDatasets()).Returns(expectedDatasets);
            var datasetInitFactory = new TestPowerBICmdletInitFactory(client.Object);
            var getDatasetCmdlet = new GetPowerBIDataset(datasetInitFactory);
            getDatasetCmdlet.InvokePowerBICmdlet();
            var datasets = datasetInitFactory.Logger.Output.ToList();
            var tableInitFactory = new TestPowerBICmdletInitFactory(client.Object);
            client.Setup(x => x.Datasets.GetTables(((Dataset)datasets[0]).Id, null)).Returns(expectedTables);
            var getTableCmdlet = new GetPowerBITable(tableInitFactory);
            getTableCmdlet.DatasetId = ((Dataset)datasets[0]).Id;

            // Act
            getTableCmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedTables, tableInitFactory);

        }
        
        private static void AssertExpectedUnitTestResults(List<Table> expectedWorkspaces, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.AreEqual(expectedWorkspaces.Count, results.Count());
            var workspaces = results.Cast<Table>().ToList();
            CollectionAssert.AreEqual(expectedWorkspaces, workspaces);
        }
    }    
}
