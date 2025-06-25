/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class AddPowerBIWorkspaceUserTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{AddPowerBIWorkspaceUser.CmdletVerb}-{AddPowerBIWorkspaceUser.CmdletName}", typeof(AddPowerBIWorkspaceUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndAddPowerBIWorkspaceUserOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);
                var emailAddress = "user1@granularcontrols1.ccsctp.net";
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Scope), PowerBIUserScope.Organization },
                    { nameof(AddPowerBIWorkspaceUser.Id), workspace.Id},
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), emailAddress },
                    { nameof(AddPowerBIWorkspaceUser.AccessRight), WorkspaceUserAccessRight.Member }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.IsTrue(updatedWorkspace.Users
                    .Any(x => x.UserPrincipalName.Equals(emailAddress, StringComparison.OrdinalIgnoreCase)
                    && x.AccessRight == WorkspaceUserAccessRight.Member.ToString()));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndAddPowerBIWorkspaceUserIndividualScope()
        {
            // TODO: Note that unlike the admin APIs, this API will throw an error when attempting to add a user that already has access to the workspace
            // This means that this end-to-end test can fail depending on when it is run with the other tests
            // This can't be elegantly solved until users are available on the non-admin GET endpoint
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);
                WorkspacesTestUtilities.AssertShouldContinueIndividualTest(workspace);
                var emailAddress = "user1@granularcontrols1.ccsctp.net";
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Scope), PowerBIUserScope.Individual },
                    { nameof(AddPowerBIWorkspaceUser.Id), workspace.Id },
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), emailAddress },
                    { nameof(AddPowerBIWorkspaceUser.AccessRight), WorkspaceUserAccessRight.Admin }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndAddPowerBIWorkspaceUser_ExplicitPrincipalType()
        {
            // Set this to the identifier of the object (App, Group, or User) you want to add to the workspace.
            const string ObjectId = "";

            // Optionally specify the Id of an existing workspace. Otherwise, the first returned workspace will be used.
            const string WorkspaceId = "";

            // The type of the object being added to the workspace.
            const WorkspaceUserPrincipalType PrincipalType = WorkspaceUserPrincipalType.App;

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, environment: PowerBIEnvironmentType.Public);

                string workspaceId = WorkspaceId;
                if (string.IsNullOrEmpty(WorkspaceId))
                {
                    var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);
                    WorkspacesTestUtilities.AssertShouldContinueIndividualTest(workspace);
                    workspaceId = workspace.Id.ToString();
                }

                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Id), workspaceId },
                    { nameof(AddPowerBIWorkspaceUser.PrincipalType), PrincipalType },
                    { nameof(AddPowerBIWorkspaceUser.Identifier), ObjectId },
                    { nameof(AddPowerBIWorkspaceUser.AccessRight), WorkspaceUserAccessRight.Contributor }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndAddPowerBIWorkspaceUserWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Id), new Guid() },
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), "user1@granularcontrols1.ccsctp.net" },
                    { nameof(AddPowerBIWorkspaceUser.AccessRight), WorkspaceUserAccessRight.Admin }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUserOrganizationScope_WorkspaceParameterSet()
        {
            // Arrange
            var workspace = new Workspace { Id = Guid.NewGuid() };
            var user = new WorkspaceUser { UserPrincipalName = "john@contoso.com", AccessRight = WorkspaceUserAccessRight.Member.ToString() };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUserAsAdmin(workspace.Id, It.Is<WorkspaceUser>(u => u.UserPrincipalName == user.UserPrincipalName && u.AccessRight == user.AccessRight)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Workspace = workspace,
                UserPrincipalName = user.UserPrincipalName,
                AccessRight = WorkspaceUserAccessRight.Member,
                ParameterSet = "Workspace",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUserOrganizationScope_IdParameterSet()
        {
            // Arrange
            var workspaceId = Guid.NewGuid();
            var user = new WorkspaceUser { UserPrincipalName = "john@contoso.com", AccessRight = WorkspaceUserAccessRight.Member.ToString() };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUserAsAdmin(workspaceId, It.Is<WorkspaceUser>(u => u.UserPrincipalName == user.UserPrincipalName && u.AccessRight == user.AccessRight)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Id = workspaceId,
                UserPrincipalName = user.UserPrincipalName,
                AccessRight = WorkspaceUserAccessRight.Member,
                ParameterSet = "UserEmailWithId",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUserIndividualScope_WorkspaceParameterSet()
        {
            // Arrange
            var workspace = new Workspace { Id = Guid.NewGuid() };
            var user = new WorkspaceUser { UserPrincipalName = "john@contoso.com", AccessRight = WorkspaceUserAccessRight.Member.ToString() };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUser(workspace.Id, It.Is<WorkspaceUser>(u => u.UserPrincipalName == user.UserPrincipalName && u.AccessRight == user.AccessRight)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Workspace = workspace,
                UserPrincipalName = user.UserPrincipalName,
                AccessRight = WorkspaceUserAccessRight.Member,
                ParameterSet = "UserEmailWithWorkspace",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUserIndividualScope_IdParameterSet()
        {
            // Arrange
            var workspaceId = Guid.NewGuid();
            var user = new WorkspaceUser { UserPrincipalName = "john@contoso.com", AccessRight = WorkspaceUserAccessRight.Member.ToString() };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUser(workspaceId, It.Is<WorkspaceUser>(u => u.UserPrincipalName == user.UserPrincipalName && u.AccessRight == user.AccessRight)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Id = workspaceId,
                UserPrincipalName = user.UserPrincipalName,
                AccessRight = WorkspaceUserAccessRight.Member,
                ParameterSet = "UserEmailWithId",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUser_PrincipalTypeApp_WithId()
        {
            // Arrange
            var workspaceId = Guid.NewGuid();
            var principalId = Guid.NewGuid();
            var user = new WorkspaceUser { Identifier = principalId.ToString(), AccessRight = WorkspaceUserAccessRight.Member.ToString(), PrincipalType = WorkspaceUserPrincipalType.App };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUser(workspaceId, It.Is<WorkspaceUser>(u => u.Identifier == user.Identifier && u.AccessRight == user.AccessRight && u.PrincipalType == user.PrincipalType)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Id = workspaceId,
                Identifier = user.Identifier,
                AccessRight = WorkspaceUserAccessRight.Member,
                PrincipalType = WorkspaceUserPrincipalType.App,
                ParameterSet = "PrincipalTypeWithId",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUser_PrincipalTypeApp_WithWorkspace()
        {
            // Arrange
            var workspace = new Workspace { Id = Guid.NewGuid() };
            var principalId = Guid.NewGuid();
            var user = new WorkspaceUser { Identifier = principalId.ToString(), AccessRight = WorkspaceUserAccessRight.Member.ToString(), PrincipalType = WorkspaceUserPrincipalType.App };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUser(workspace.Id, It.Is<WorkspaceUser>(u => u.Identifier == user.Identifier && u.AccessRight == user.AccessRight && u.PrincipalType == user.PrincipalType)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Workspace = workspace,
                Identifier = user.Identifier,
                AccessRight = WorkspaceUserAccessRight.Member,
                PrincipalType = WorkspaceUserPrincipalType.App,
                ParameterSet = "PrincipalTypeWithWorkspace",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void AddPowerBIWorkspaceUser_PrincipalTypeGroup_WithId()
        {
            // Arrange
            var workspaceId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var user = new WorkspaceUser { Identifier = groupId.ToString(), AccessRight = WorkspaceUserAccessRight.Viewer.ToString(), PrincipalType = WorkspaceUserPrincipalType.Group };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .AddWorkspaceUser(workspaceId, It.Is<WorkspaceUser>(u => u.Identifier == user.Identifier && u.AccessRight == user.AccessRight && u.PrincipalType == user.PrincipalType)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIWorkspaceUser(initFactory)
            {
                Id = workspaceId,
                Identifier = user.Identifier,
                AccessRight = WorkspaceUserAccessRight.Viewer,
                PrincipalType = WorkspaceUserPrincipalType.Group,
                ParameterSet = "PrincipalTypeWithId",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
