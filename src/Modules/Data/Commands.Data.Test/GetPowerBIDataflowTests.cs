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
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data
{
    [TestClass]
    public class GetPowerBIDataflowTests
    {
        public static CmdletInfo GetPowerBIDataflowCmdletInfo { get; } = new CmdletInfo($"{GetPowerBIDataflow.CmdletVerb}-{GetPowerBIDataflow.CmdletName}", typeof(GetPowerBIDataflow));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIDataflowOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIDataflowCmdletInfo).AddParameter("Scope", "Organization");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public void GetPowerBIDataflowIndividualScope_ListParameterSet()
        {
            // Arrange
            var expectedDataflows = new List<Dataflow> { new Dataflow { Id = Guid.NewGuid(), Name = "TestDataflow" } };
            var workspaceId = Guid.NewGuid();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Dataflows.GetDataflows(workspaceId)).Returns(expectedDataflows);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIDataflow(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                ParameterSet = "List",
                WorkspaceId = workspaceId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            initFactory.AssertExpectedUnitTestResults(expectedDataflows);
        }
    }
}
