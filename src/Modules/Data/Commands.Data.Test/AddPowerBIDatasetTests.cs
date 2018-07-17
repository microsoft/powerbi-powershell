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
    public class AddPowerBIDatasetTests
    {
        private static CmdletInfo NewPowerBIColumnCmdletInfo => new CmdletInfo($"{NewPowerBIColumn.CmdletVerb}-{NewPowerBIColumn.CmdletName}", typeof(NewPowerBIColumn));
        private static CmdletInfo NewPowerBITableCmdletInfo => new CmdletInfo($"{NewPowerBITable.CmdletVerb}-{NewPowerBITable.CmdletName}", typeof(NewPowerBITable));
        private static CmdletInfo NewPowerBIDatasetCmdletInfo => new CmdletInfo($"{NewPowerBIDataset.CmdletVerb}-{NewPowerBIDataset.CmdletName}", typeof(NewPowerBIDataset));
        private static CmdletInfo AddPowerBIDatasetCmdletInfo => new CmdletInfo($"{AddPowerBIDataset.CmdletVerb}-{AddPowerBIDataset.CmdletName}", typeof(AddPowerBIDataset));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIDataset()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(NewPowerBIColumnCmdletInfo).AddParameter(nameof(NewPowerBIColumn.Name), "Col1").AddParameter(nameof(NewPowerBIColumn.DataType), PowerBIDataType.String);
                var col1 = ps.Invoke();
                ps.Commands.Clear();
                ps.AddCommand(NewPowerBIColumnCmdletInfo).AddParameter(nameof(NewPowerBIColumn.Name), "Col2").AddParameter(nameof(NewPowerBIColumn.DataType), PowerBIDataType.Boolean);
                var col2 = ps.Invoke();
                ps.Commands.Clear();
                var columns = new List<Column>() { col1.First().BaseObject as Column, col2.First().BaseObject as Column };
                ps.AddCommand(NewPowerBITableCmdletInfo).AddParameter(nameof(NewPowerBITable.Name), "Table1").AddParameter(nameof(NewPowerBITable.Columns), columns);
                var table1 = ps.Invoke();
                var tables = new List<Table>() { table1.First().BaseObject as Table };
                ps.Commands.Clear();
                ps.AddCommand(NewPowerBIDatasetCmdletInfo).AddParameter(nameof(NewPowerBIDataset.Name), "MyDataSet").AddParameter(nameof(NewPowerBIDataset.Tables), tables);
                var dataset = ps.Invoke();
                ps.Commands.Clear();

                ps.AddCommand(AddPowerBIDatasetCmdletInfo)
                    .AddParameter(nameof(AddPowerBIDataset.Dataset), dataset.First().BaseObject as Dataset);
                
                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void AddPowerBIDataset_DatasetParameterSetName()
        {
            var expectedDataset = new Dataset { Id = Guid.NewGuid(), Name = "TestDataset" };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.AddDataset(expectedDataset, null)).Returns(expectedDataset);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIDataset(initFactory)
            {
                Dataset = expectedDataset
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedDataset, client, initFactory);
        }      
    }    
}
