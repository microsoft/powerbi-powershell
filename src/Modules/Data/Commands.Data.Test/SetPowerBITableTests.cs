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
    public class SetPowerBITableTests
    {
        private static CmdletInfo SetPowerBITestCmdletInfo => new CmdletInfo($"{SetPowerBITable.CmdletVerb}-{SetPowerBITable.CmdletName}", typeof(SetPowerBITable));
        private static CmdletInfo GetPowerBITableCmdletInfo => new CmdletInfo($"{GetPowerBITable.CmdletVerb}-{GetPowerBITable.CmdletName}", typeof(GetPowerBITable));
        private static CmdletInfo GetPowerBIDatasetCmdletInfo => new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));
   
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetPowerBIDataset()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, nameof(PowerBIEnvironmentType.Public));
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter("Scope", PowerBIUserScope.Individual);
                var dataSetResult = ps.Invoke();
                ps.Commands.Clear();
                var datasetId = dataSetResult.Where(x => (bool)x.Members["addRowsAPIEnabled"].Value == true).First().Members["Id"].Value;
                ps.AddCommand(GetPowerBITableCmdletInfo)
                    .AddParameter("DatasetId", datasetId);
                var tableResult = ps.Invoke();
                ps.Commands.Clear();

                var table = tableResult.First().BaseObject as Table;
                var cols = new List<Column>();
                cols.Add(new Column { Name = "newcol", DataType = PowerBIDataType.String.ToString() });
                table.Columns = cols;
                ps.AddCommand(SetPowerBITestCmdletInfo)
                    .AddParameter("Table", table)
                    .AddParameter("DatasetId", datasetId);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void SetPowerBITable_DatasetIdParameterSetName()
        {
            var expectedTable = new Table { Name = "TestTable" };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.UpdateTable(expectedTable, Guid.Empty, null)).Returns(expectedTable);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBITable(initFactory);

            cmdlet.Table = expectedTable;
            cmdlet.DatasetId = Guid.Empty;

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            Assert.AreEqual(expectedTable.Name, (initFactory.Logger.Output.First() as Table).Name);
        }
    }    
}
