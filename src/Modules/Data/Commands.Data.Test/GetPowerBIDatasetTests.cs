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

namespace Microsoft.PowerBI.Commands.Data
{
    [TestClass]
    public class GetPowerBIDatasetTests
    {
        public static CmdletInfo GetPowerBIDatasetCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDataset.CmdletVerb}-{GetPowerBIDataset.CmdletName}", typeof(GetPowerBIDataset));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIDatasetOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIDatasetCmdletInfo).AddParameter("Scope", "Organization");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void GetPowerBIDatasetIndividualScope_ListParameterSet()
        {
            // Arrange
            var expectedDatasets = new List<Dataset> { new Dataset { Id = Guid.NewGuid(), Name = "TestDataset" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets.GetDatasets()).Returns(expectedDatasets);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataset(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                ParameterSet = "List",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDatasets);
        }
    }
}
