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
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class AddPowerBIRowTests
    {
        private static CmdletInfo AddPowerBIRowCmdletInfo => new CmdletInfo($"{AddPowerBIRow.CmdletVerb}-{AddPowerBIRow.CmdletName}", typeof(AddPowerBIRow));
        private static CmdletInfo GetPowerBITableCmdletInfo => new CmdletInfo($"{GetPowerBITable.CmdletVerb}-{GetPowerBITable.CmdletName}", typeof(GetPowerBITable));
        private static CmdletInfo GetPowerBIDatasetCmdletInfo => new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));
   
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIRows()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter(nameof(GetPowerBITable.Scope), PowerBIUserScope.Individual);
                var dataSetResults = ps.Invoke();
                ps.Commands.Clear();
                var datasetId = dataSetResults.Where(x => (bool)x.Members["addRowsAPIEnabled"].Value == true).First().Members["Id"].Value;
                ps.AddCommand(GetPowerBITableCmdletInfo)
                    .AddParameter(nameof(GetPowerBITable.DatasetId), datasetId);
                var tableResults = ps.Invoke();
                ps.Commands.Clear();

                var table = tableResults.First().BaseObject as Table;

                var row1 = new PSObject();
                row1.Members.Add(new PSNoteProperty("Col1","Data1"));
                row1.Members.Add(new PSNoteProperty("Col2", true));
                var row2 = new PSObject();
                row2.Members.Add(new PSNoteProperty("Col1", "Data2"));
                row2.Members.Add(new PSNoteProperty("Col2", false));
                var rows = new List<PSObject>();
                rows.Add(row1);
                rows.Add(row2);
                ps.AddCommand(AddPowerBIRowCmdletInfo)
                    .AddParameter(nameof(AddPowerBIRow.DatasetId), datasetId)
                    .AddParameter(nameof(AddPowerBIRow.TableName), table.Name)
                    .AddParameter(nameof(AddPowerBIRow.Rows), rows);

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
        public void EndToEndAddPowerBIRowsFromCSV()
        {
            var csvPath = @"path_to_csv_file";
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter(nameof(GetPowerBITable.Scope), PowerBIUserScope.Individual);
                var dataSetResults = ps.Invoke();
                ps.Commands.Clear();
                var datasetId = dataSetResults.Where(x => (bool)x.Members["addRowsAPIEnabled"].Value == true).First().Members["Id"].Value;
                ps.AddCommand(GetPowerBITableCmdletInfo)
                    .AddParameter(nameof(GetPowerBITable.DatasetId), datasetId);
                var tableResults = ps.Invoke();
                ps.Commands.Clear();

                var table = tableResults.First().BaseObject as Table;

                ps.AddCommand("Import-CSV").AddParameter("Path",csvPath);
                var rows = ps.Invoke();
                ps.Commands.Clear();
                ps.AddCommand(AddPowerBIRowCmdletInfo)
                    .AddParameter(nameof(AddPowerBIRow.DatasetId), datasetId)
                    .AddParameter(nameof(AddPowerBIRow.TableName), table.Name)
                    .AddParameter(nameof(AddPowerBIRow.Rows), rows);

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
        public void EndToEndAddPowerBIRowsFromArray()
        {           
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter(nameof(GetPowerBITable.Scope), PowerBIUserScope.Individual);
                var dataSetResults = ps.Invoke();
                ps.Commands.Clear();
                var datasetId = dataSetResults.Where(x => (bool)x.Members["addRowsAPIEnabled"].Value == true).First().Members["Id"].Value;
                ps.AddCommand(GetPowerBITableCmdletInfo)
                    .AddParameter(nameof(GetPowerBITable.DatasetId), datasetId);
                var tableResults = ps.Invoke();
                ps.Commands.Clear();

                var table = tableResults.First().BaseObject as Table;

                ps.AddScript("@{\"Col1\"=\"Value1\";\"Col2\"=$true},@{\"Col1\"=\"Value2\";\"Col2\"=$false}");
                var rows = ps.Invoke();
                ps.Commands.Clear();
                ps.AddCommand(AddPowerBIRowCmdletInfo)
                    .AddParameter(nameof(AddPowerBIRow.DatasetId), datasetId)
                    .AddParameter(nameof(AddPowerBIRow.TableName), table.Name)
                    .AddParameter(nameof(AddPowerBIRow.Rows), rows);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }
        [TestMethod]
        public void AddPowerBIRows_DatasetIdParameterSetName()
        {
            var datasetId = new Guid();
            var table = new Table { Name = "TestTable" };
            var row1 = new PSObject();
            row1.Members.Add(new PSNoteProperty("Col1", "Data1"));
            row1.Members.Add(new PSNoteProperty("Col2", true));
            var row2 = new PSObject();
            row2.Members.Add(new PSNoteProperty("Col1", "Data2"));
            row2.Members.Add(new PSNoteProperty("Col2", false));
            var rows = new List<PSObject>();
            rows.Add(row1);
            rows.Add(row2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.AddRows(datasetId, table.Name, rows, null));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIRow(initFactory);

            cmdlet.DatasetId = datasetId;
            cmdlet.TableName = table.Name;
            cmdlet.Rows = rows;

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedNoOutputForUnitTestResults(client, initFactory);
        }
    }
}
