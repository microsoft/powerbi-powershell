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

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class RemovePowerBIRowsTests
    {
        private static CmdletInfo RemovePowerBIRowsCmdletInfo => new CmdletInfo($"{RemovePowerBIRows.CmdletVerb}-{RemovePowerBIRows.CmdletName}", typeof(RemovePowerBIRows));
        private static CmdletInfo GetPowerBITableCmdletInfo => new CmdletInfo($"{GetPowerBITable.CmdletVerb}-{GetPowerBITable.CmdletName}", typeof(GetPowerBITable));
        private static CmdletInfo GetPowerBIDatasetCmdletInfo => new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));
   
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemovePowerBIRows()
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

                ps.AddCommand(RemovePowerBIRowsCmdletInfo)
                    .AddParameter(nameof(RemovePowerBIRows.DatasetId), datasetId)
                    .AddParameter(nameof(RemovePowerBIRows.TableName), table.Name);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void RemovePowerBIRows_DatasetIdParameterSetName()
        {
            var datasetId = new Guid();
            var table = new Table { Name = "TestTable" };
    
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.DeleteRows(datasetId.ToString(), table.Name, null)).Returns(null);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RemovePowerBIRows(initFactory);

            cmdlet.DatasetId = datasetId;
            cmdlet.TableName = table.Name;
    
            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(null, client, initFactory);
        }
    }
}
