/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class GetPowerBIWorkspaceMigrationStatusTests
    {
        [TestMethod]
        public void GetWorkspaceLastMigrationstatus()
        {
            // Arrange
            var workspace = new Workspace { Id = Guid.NewGuid() };
            var expectedResponse = new WorkspaceLastMigrationStatus();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspaceLastMigrationStatus(workspace.Id))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspaceMigrationStatus(initFactory)
            {
                Workspace = workspace,
                ParameterSet = "Workspace",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
