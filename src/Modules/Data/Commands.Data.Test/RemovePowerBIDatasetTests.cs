/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Datasets;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class RemovePowerBIDatasetTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemovePowerBIDataset()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public); // If login is needed
                ps.AddCommand(new CmdletInfo($"{RemovePowerBIDataset.CmdletVerb}-{RemovePowerBIDataset.CmdletName}", typeof(RemovePowerBIDataset)));
                ps.AddParameter("Id", "fce8abb5-192b-4be2-b75e-b43bb93d8943");
                ps.AddParameter("WorkspaceId", "kjsdfjs;sf");
                var result = ps.Invoke();

                // Add asserts to verify

                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void RemovePowerBIDatasetTest()
        {
            // Arrange 
            var datasetID = Guid.NewGuid();
            object expectedResponse = null;
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Datasets
                .DeleteDataset(datasetID))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RemovePowerBIDataset(initFactory)
            {
                Id = datasetID
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
