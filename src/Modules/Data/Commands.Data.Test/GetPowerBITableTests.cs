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
        public static CmdletInfo GetPowerBITableCmdletInfo { get; } = new CmdletInfo($"{GetPowerBITable.CmdletVerb}-{GetPowerBITable.CmdletName}", typeof(GetPowerBITable));
        public static CmdletInfo GetPowerBIDatasetCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void GetPowerBITable_DatasetIdSetName()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, "Public");
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter("Scope", "Individual");
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
        public void GetPowerBITable_DatasetSetName()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, "Public");
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter("Scope", "Individual");
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
    }
}
