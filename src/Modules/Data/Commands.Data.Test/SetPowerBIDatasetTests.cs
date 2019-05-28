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
    public class SetPowerBIDatasetTests
    {
        private static CmdletInfo SetPowerBIDatasetTestCmdletInfo => new CmdletInfo($"{SetPowerBIDataset.CmdletVerb}-{SetPowerBIDataset.CmdletName}", typeof(SetPowerBIDataset));
        private static CmdletInfo GetPowerBIDatasetCmdletInfo => new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetPowerBIDataset()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter(nameof(GetPowerBITable.Scope), PowerBIUserScope.Individual);
                var dataSetResults = ps.Invoke();
                ps.Commands.Clear();

                ps.AddCommand(SetPowerBIDatasetTestCmdletInfo)
                    .AddParameter(nameof(SetPowerBIDataset.Id), dataSetResults.First().Members["Id"].Value)
                    .AddParameter(nameof(SetPowerBIDataset.TargetStorageMode), DatasetStorageMode.PremiumFiles);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void SetPowerBIDataset_DatasetIdParameterSetName()
        {
            var datasetId = Guid.NewGuid();

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.PatchDataset(
                datasetId,
                It.Is<PatchDatasetRequest>(pdr => pdr.TargetStorageMode == DatasetStorageMode.PremiumFiles),
                Guid.Empty));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBIDataset(initFactory);

            cmdlet.Id = datasetId;
            cmdlet.TargetStorageMode = DatasetStorageMode.PremiumFiles;

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedNoOutputForUnitTestResults(client, initFactory);
        }
    }
}
