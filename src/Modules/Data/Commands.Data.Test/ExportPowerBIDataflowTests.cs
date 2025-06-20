/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Data.Test
{
    [TestClass]
    public class ExportPowerBIDataflowTests
    {
        public static CmdletInfo ExportPowerBIDataflowCmdletInfo { get; } = new CmdletInfo($"{ExportPowerBIDataflow.CmdletVerb}-{ExportPowerBIDataflow.CmdletName}", typeof(ExportPowerBIDataflow));

        private const string c_dataflowModel = "./dataflowModel.json";
        private const string c_ExportedDataflowModel = "./ExportedDataflowModel.json";

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndExportPowerBIDataflowOrganizationScope_ByDataflowId()
        {
            /*
             * Requirement to run test:
             * Need at least one dataflow assigned to the user logging into the test.
             */
            try
            {
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    // Arrange
                    ProfileTestUtilities.ConnectToPowerBI(ps);
                    ps.AddCommand(GetPowerBIDataflowTests.GetPowerBIDataflowCmdletInfo).AddParameter(nameof(ExportPowerBIDataflow.Scope), PowerBIUserScope.Organization);

                    var existingDataflows = ps.Invoke();
                    TestUtilities.AssertNoCmdletErrors(ps);
                    ps.Commands.Clear();

                    if (!existingDataflows.Any())
                    {
                        Assert.Inconclusive("No dataflows returned. Verify you have dataflows under your logged in user.");
                    }

                    var testDataflow = existingDataflows.Select(d => (Dataflow)d.BaseObject).FirstOrDefault();

                    var parameters = new Dictionary<string, object>()
                    {
                        { nameof(ExportPowerBIDataflow.Id), testDataflow.Id },
                        { nameof(ExportPowerBIDataflow.Scope), PowerBIUserScope.Organization },
                        { nameof(ExportPowerBIDataflow.OutFile), c_ExportedDataflowModel }
                    };

                    ps.AddCommand(ExportPowerBIDataflowCmdletInfo).AddParameters(parameters);

                    // Act
                    var result = ps.Invoke();

                    // Assert
                    TestUtilities.AssertNoCmdletErrors(ps);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(File.Exists(c_ExportedDataflowModel));
                }
            }
            finally
            {
                // always delete the test's exported file
                File.Delete(c_ExportedDataflowModel);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndExportPowerBIDataflowOrganizationScope_ByDataflow()
        {
            /*
             * Requirement to run test:
             * Need at least one dataflow assigned to the user logging into the test.
             */
            try
            {
                using (var ps = System.Management.Automation.PowerShell.Create())
                {
                    // Arrange
                    ProfileTestUtilities.ConnectToPowerBI(ps);
                    ps.AddCommand(GetPowerBIDataflowTests.GetPowerBIDataflowCmdletInfo).AddParameter(nameof(ExportPowerBIDataflow.Scope), PowerBIUserScope.Organization);

                    var existingDataflows = ps.Invoke();
                    TestUtilities.AssertNoCmdletErrors(ps);
                    ps.Commands.Clear();

                    if (!existingDataflows.Any())
                    {
                        Assert.Inconclusive("No dataflows returned. Verify you have dataflows under your logged in user.");
                    }

                    var testDataflow = existingDataflows.Select(d => (Dataflow)d.BaseObject).FirstOrDefault();

                    var parameters = new Dictionary<string, object>()
                    {
                        { nameof(ExportPowerBIDataflow.Dataflow), testDataflow },
                        { nameof(ExportPowerBIDataflow.Scope), PowerBIUserScope.Organization },
                        { nameof(ExportPowerBIDataflow.OutFile), c_ExportedDataflowModel }
                    };

                    ps.AddCommand(ExportPowerBIDataflowCmdletInfo).AddParameters(parameters);

                    // Act
                    var result = ps.Invoke();

                    // Assert
                    TestUtilities.AssertNoCmdletErrors(ps);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(File.Exists(c_ExportedDataflowModel));
                }
            }
            finally
            {
                // always delete the test's exported file
                File.Delete(c_ExportedDataflowModel);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndExportPowerBIDataflowWithoutLoginFail()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(GetPowerBIDataflowDatasource.DataflowId), Guid.NewGuid() },
                    { nameof(GetPowerBIDataflowDatasource.WorkspaceId), Guid.NewGuid() },
                    { nameof(ExportPowerBIDataflow.OutFile), c_ExportedDataflowModel }
                };
                ps.AddCommand(ExportPowerBIDataflowCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void ExportPowerBIDataflowOrganizationScope_ByDataflowId()
        {
            try
            {
                // Arrange
                var dataflowId = Guid.NewGuid();
                var client = new Mock<IPowerBIApiClient>();

                using (var expecedDataflowStream = File.OpenRead(c_dataflowModel))
                {
                    client.Setup(x => x.Dataflows.ExportDataflowAsAdmin(dataflowId)).Returns(expecedDataflowStream);

                    var initFactory = new TestPowerBICmdletInitFactory(client.Object);
                    var cmdlet = new ExportPowerBIDataflow(initFactory)
                    {
                        Scope = PowerBIUserScope.Organization,
                        ParameterSet = "Id",
                        Id = dataflowId,
                        OutFile = c_ExportedDataflowModel
                    };

                    // Act
                    cmdlet.InvokePowerBICmdlet();
                }

                // Assert
                Assert.IsTrue(File.Exists(c_ExportedDataflowModel));
            }
            finally
            {
                // always delete the test's exported file
                File.Delete(c_ExportedDataflowModel);
            }
        }

        [TestMethod]
        public void ExportPowerBIDataflowIndividualScope_ByDataflowId()
        {
            try
            {
                // Arrange
                var workspaceId = Guid.NewGuid();
                var dataflowId = Guid.NewGuid();
                var client = new Mock<IPowerBIApiClient>();

                using (var expecedDataflowStream = File.OpenRead(c_dataflowModel))
                {
                    client.Setup(x => x.Dataflows.GetDataflow(workspaceId, dataflowId)).Returns(expecedDataflowStream);

                    var initFactory = new TestPowerBICmdletInitFactory(client.Object);
                    var cmdlet = new ExportPowerBIDataflow(initFactory)
                    {
                        Scope = PowerBIUserScope.Individual,
                        ParameterSet = "Id",
                        WorkspaceId = workspaceId,
                        Id = dataflowId,
                        OutFile = c_ExportedDataflowModel
                    };

                    // Act
                    cmdlet.InvokePowerBICmdlet();
                }

                // Assert
                Assert.IsTrue(File.Exists(c_ExportedDataflowModel));
            }
            finally
            {
                // always delete the test's exported file
                File.Delete(c_ExportedDataflowModel);
            }
        }

        [TestMethod]
        public void ExportPowerBIDataflowOrganizationScope_ByDataflow()
        {
            try
            {
                // Arrange
                var dataflow = new Dataflow { Id = Guid.NewGuid(), Name = "DataflowName" };
                var client = new Mock<IPowerBIApiClient>();

                using (var expecedDataflowStream = File.OpenRead(c_dataflowModel))
                {
                    client.Setup(x => x.Dataflows.ExportDataflowAsAdmin(dataflow.Id)).Returns(expecedDataflowStream);

                    var initFactory = new TestPowerBICmdletInitFactory(client.Object);
                    var cmdlet = new ExportPowerBIDataflow(initFactory)
                    {
                        Scope = PowerBIUserScope.Organization,
                        ParameterSet = "Id",
                        Dataflow = dataflow,
                        OutFile = c_ExportedDataflowModel
                    };

                    // Act
                    cmdlet.InvokePowerBICmdlet();
                }

                // Assert
                Assert.IsTrue(File.Exists(c_ExportedDataflowModel));
            }
            finally
            {
                // always delete the test's exported file
                File.Delete(c_ExportedDataflowModel);
            }
        }

        [TestMethod]
        public void ExportPowerBIDataflowIndividualScope_ByDataflow()
        {
            try
            {
                // Arrange
                var workspaceId = Guid.NewGuid();
                var dataflow = new Dataflow { Id = Guid.NewGuid(), Name = "DataflowName" };
                var client = new Mock<IPowerBIApiClient>();

                using (var expecedDataflowStream = File.OpenRead(c_dataflowModel))
                {
                    client.Setup(x => x.Dataflows.GetDataflow(workspaceId, dataflow.Id)).Returns(expecedDataflowStream);

                    var initFactory = new TestPowerBICmdletInitFactory(client.Object);
                    var cmdlet = new ExportPowerBIDataflow(initFactory)
                    {
                        Scope = PowerBIUserScope.Individual,
                        ParameterSet = "Id",
                        WorkspaceId = workspaceId,
                        Dataflow = dataflow,
                        OutFile = c_ExportedDataflowModel
                    };

                    // Act
                    cmdlet.InvokePowerBICmdlet();
                }

                // Assert
                Assert.IsTrue(File.Exists(c_ExportedDataflowModel));
            }
            finally
            {
                // always delete the test's exported file
                File.Delete(c_ExportedDataflowModel);
            }
        }
    }
}
