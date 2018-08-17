/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class AddPowerBIRowsTests
    {
        private static CmdletInfo AddPowerBIRowsCmdletInfo => new CmdletInfo($"{AddPowerBIRows.CmdletVerb}-{AddPowerBIRows.CmdletName}", typeof(AddPowerBIRows));
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

                var row1 = new JObject();
                row1["Col1"] = "Data1";
                row1["Col2"] = true;
                var row2 = new JObject();
                row2["Col1"] = "Data2";
                row2["Col2"] = false;
                var rows = new JArray();
                rows.Add(row1);
                rows.Add(row2);
                ps.AddCommand(AddPowerBIRowsCmdletInfo)
                    .AddParameter(nameof(AddPowerBIRows.DatasetId), datasetId)
                    .AddParameter(nameof(AddPowerBIRows.TableName), table.Name)
                    .AddParameter(nameof(AddPowerBIRows.Rows), rows);

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
            var row1 = new JObject();
            row1["Col1"] = "Data1";
            row1["Col2"] = true;
            var row2 = new JObject();
            row2["Col1"] = "Data2";
            row2["Col2"] = false;
            var rows = new JArray();
            rows.Add(row1);
            rows.Add(row2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.AddRows(datasetId.ToString(), table.Name, rows, null)).Returns(null);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIRows(initFactory);

            cmdlet.DatasetId = datasetId;
            cmdlet.TableName = table.Name;
            cmdlet.Rows = rows;

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(null, client, initFactory);
        }
    }
}
