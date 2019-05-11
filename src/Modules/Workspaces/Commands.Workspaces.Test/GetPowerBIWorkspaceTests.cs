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
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class GetPowerBIWorkspaceTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesOrganizationScope()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameter(nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesAllAndOrganizationScope()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.All), true }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);

                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesAllAndIndividualScope()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) and login as a user.
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Individual },
                        { nameof(GetPowerBIWorkspace.All), true }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);

                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify the user has workspaces.");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesAllAndOrganizationScopeAndUser()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) with the given user and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceWithUsersInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);
                var userEmailAddress = workspace.Users.FirstOrDefault().UserPrincipalName;
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.All), true },
                        { nameof(GetPowerBIWorkspace.User), userEmailAddress }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);

                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you have workspaces with the specified user in your organization.");
                }

                Assert.IsNotNull(results.Select(x => (Workspace)x.BaseObject).FirstOrDefault(w => w.Id == workspace.Id));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesIndividualScope()
        {
            /*
             * Test requires at least one workspace (group or preview workspace)
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameter(nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Individual);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you are assigned or own any workspaces.");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesOrganizationScopeAndFirst()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.First), 1 }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
                }

                Assert.AreEqual(1, results.Count);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesIndividualScopeAndFirst()
        {
            /*
             * Test requires at least one workspace (group or preview workspace)
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Individual },
                        { nameof(GetPowerBIWorkspace.First), 1 }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you are assigned or own any workspaces.");
                }

                Assert.AreEqual(1, results.Count);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesOrganizationScopeAndFilter()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Organization);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);
                var filterQuery = string.Format("name eq '{0}'", workspace.Name);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.Filter), filterQuery }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());

                // Arrange
                ps.Commands.Clear();
                parameters[nameof(GetPowerBIWorkspace.Filter)] = string.Format("name eq '{0}'", TestUtilities.GetRandomString());
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsFalse(results.Any());
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesIndividualScopeAndFilter()
        {
            /*
             * Test requires at least one workspace (group or preview workspace) and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);
                WorkspacesTestUtilities.AssertShouldContinueIndividualTest(workspace);
                var filterQuery = string.Format("name eq '{0}'", workspace.Name);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Individual },
                        { nameof(GetPowerBIWorkspace.Filter), filterQuery }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());

                // Arrange
                ps.Commands.Clear();
                parameters[nameof(GetPowerBIWorkspace.Filter)] = string.Format("name eq '{0}'", TestUtilities.GetRandomString());
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsFalse(results.Any());
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesOrganizationScopeAndUser()
        {
            /*
             * Test requires a preview workspace (v2) to exist and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceWithUsersInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);
                var userEmailAddress = workspace.Users.FirstOrDefault().UserPrincipalName;
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.User), userEmailAddress }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());

                // Arrange
                ps.Commands.Clear();
                parameters[nameof(GetPowerBIWorkspace.User)] = TestUtilities.GetRandomString();
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsFalse(results.Any());
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesOrganizationScopeAndDeleted()
        {
            /*
             * Test requires at least one deleted workspace and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var deletedWorkspace = WorkspacesTestUtilities.GetFirstDeletedWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(deletedWorkspace);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.Deleted), true }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
                }

                var deletedWorkspaces = results.Select(x => (Workspace)x.BaseObject);
                Assert.IsTrue(deletedWorkspaces.Any(x => x.Id == deletedWorkspace.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndGetWorkspacesWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void GetWorkspacesOrganizationScope()
        {
            // Arrange
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllDeleted()
        {
            // Arrange
            var expectedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestDeletedWorkspace", State = WorkspaceState.Deleted };
            var allWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", State = WorkspaceState.Active }, expectedWorkspace};
            
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                Deleted = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { expectedWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllOrphaned()
        {
            // Arrange
            var deletedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestDeletedWorkspace", State = WorkspaceState.Deleted };
            var user1 = new WorkspaceUser { UserPrincipalName = "randomUser1@pbi.com", AccessRight = WorkspaceUserAccessRight.Member };
            var user2 = new WorkspaceUser { UserPrincipalName = "randomUser2@pbi.com", AccessRight = WorkspaceUserAccessRight.Contributor };
            var orphanedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user1, user2 } };
            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin };
            var normalWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } };

            var allWorkspaces = new List<Workspace> { normalWorkspace, orphanedWorkspace, deletedWorkspace };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                Orphaned = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { orphanedWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllOrphanedWithNullUsers()
        {
            // Arrange
            var expectedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active };
            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin};
            var allWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } }, expectedWorkspace };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                Orphaned = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { expectedWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllOrphanedWithEmptyUsers()
        {
            // Arrange
            var expectedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser>() };
            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin };
            var allWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } }, expectedWorkspace };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                Orphaned = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { expectedWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllOrphanedWithNonAdminUsers()
        {
            // Arrange
            var user1 = new WorkspaceUser { UserPrincipalName = "randomUser1@pbi.com", AccessRight = WorkspaceUserAccessRight.Member };
            var user2 = new WorkspaceUser { UserPrincipalName = "randomUser2@pbi.com", AccessRight = WorkspaceUserAccessRight.Contributor };
            var expectedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user1, user2 } };

            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin };
            var allWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } }, expectedWorkspace };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                Orphaned = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { expectedWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllAndUser()
        {
            // Arrange
            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin };
            var user1 = new WorkspaceUser { UserPrincipalName = "randomUser1@pbi.com", AccessRight = WorkspaceUserAccessRight.Member };
            var user2 = new WorkspaceUser { UserPrincipalName = "randomUser2@pbi.com", AccessRight = WorkspaceUserAccessRight.Contributor };
            var orphanedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user1, user2 } };
            var deletedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestDeletedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Deleted };
            var normalWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } };

            var allWorkspaces = new List<Workspace> { normalWorkspace, deletedWorkspace, orphanedWorkspace };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                User = "randomUser@pbi.com",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { normalWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllOrphanedAndUser()
        {
            // Arrange
            var user1 = new WorkspaceUser { UserPrincipalName = "randomUser1@pbi.com", AccessRight = WorkspaceUserAccessRight.Member };
            var user2 = new WorkspaceUser { UserPrincipalName = "randomUser2@pbi.com", AccessRight = WorkspaceUserAccessRight.Contributor };
            var expectedOrphanedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user1, user2 } };
            var expectedDeletedWorkspace = new Workspace { Id = Guid.NewGuid(), Name = "TestDeletedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Deleted };

            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin };
            var allWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } }, expectedDeletedWorkspace, expectedOrphanedWorkspace };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                User = "randomUser1@pbi.com",
                Orphaned = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { expectedOrphanedWorkspace }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllOrphanedWithGroup()
        {
            // Arrange
            var user1 = new WorkspaceUser { UserPrincipalName = "randomUser1@pbi.com", AccessRight = WorkspaceUserAccessRight.Member };
            var user2 = new WorkspaceUser { UserPrincipalName = "randomUser2@pbi.com", AccessRight = WorkspaceUserAccessRight.Contributor };
            var expectedOrphanedWorkspaceOne = new Workspace { Id = Guid.NewGuid(), Name = "TestOrphanedWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user1, user2 } };
            var expectedOrphanedWorkspaceTwo = new Workspace { Id = Guid.NewGuid(), Name = "TestDeletedWorkspace", Type = WorkspaceType.Group, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user1 } };

            var user = new WorkspaceUser { UserPrincipalName = "randomUser@pbi.com", AccessRight = WorkspaceUserAccessRight.Admin };
            var allWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace", Type = WorkspaceType.Workspace, State = WorkspaceState.Active, Users = new List<WorkspaceUser> { user } }, expectedOrphanedWorkspaceOne, expectedOrphanedWorkspaceTwo };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", null, It.IsAny<int>())).Returns(allWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                All = true,
                Orphaned = true
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(new List<Workspace> { expectedOrphanedWorkspaceOne }, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesAllParameterLoopTest()
        {
            // Arrange
            var expectedWorkspaces = new List<Workspace>();
            for (int i=1; i<=10500; i++)
            {
                expectedWorkspaces.Add(new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace_" + i });
            }

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(5000, "users", It.IsAny<string>(), 0)).Returns(expectedWorkspaces.Take(5000));
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(5000, "users", It.IsAny<string>(), 5000)).Returns(expectedWorkspaces.GetRange(5000, 5000));
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(5000, "users", It.IsAny<string>(), 10000)).Returns(expectedWorkspaces.Skip(10000));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                All = true,
                Scope = PowerBIUserScope.Organization,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(m => m.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesOrganizationScopeSupportsFilterTopSkip()
        {
            // Arrange
            var filter = "name eq 'n/a'";
            var first = 2;
            var skip = 5;
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(first, "users", filter, skip)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Filter = filter,
                First = first,
                Skip = skip,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesOrganizationScopeSupportsId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var filter = $"tolower(id) eq '{id}'";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", filter, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Id = id,
                ParameterSet = "Id",
            };
            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }
       
        [TestMethod]
        public void GetWorkspacesOrganizationScopeSupportsName()
        {
            // Arrange
            var name = "Test";
            var filter = $"tolower(name) eq '{name.ToLower()}'";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = name } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", filter, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Name = name,
                ParameterSet = "Name",
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesOrganizationScopeSupportsUser()
        {
            // Arrange
            var user = "JOHN@contoso.com";
            var filter = $"users/any(u: tolower(u/emailAddress) eq '{user.ToLower()}')";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", filter, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                User = user,
            };
            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesOrganizationScopeSupportsDeleted()
        {
            // Arrange
            var filter = string.Format("state eq '{0}'", WorkspaceState.Deleted);
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", filter, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Deleted = true,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesOrganizationScopeSupportsOrphaned()
        {
            // Arrange
            var filter = $"(state ne '{WorkspaceState.Deleted}') and ((not users/any()) or (not users/any(u: u/groupUserAccessRight eq Microsoft.PowerBI.ServiceContracts.Api.GroupUserAccessRight'Admin')))";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", filter, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Orphaned = true,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScope()
        {
            // Arrange
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspaces(null, It.IsAny<int>(), null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScopeSupportsFilterTopSkip()
        {
            // Arrange
            var filter = "name eq 'n/a'";
            var first = 2;
            var skip = 5;
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspaces(filter, first, skip)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Filter = filter,
                First = first,
                Skip = skip,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScopeSupportsId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var filter = $"tolower(id) eq '{id}'";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspaces(filter, It.IsAny<int>(), null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Id = id,
                ParameterSet = "Id",
            };
            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScopeSupportsName()
        {
            // Arrange
            var name = "Test";
            var filter = $"tolower(name) eq '{name.ToLower()}'";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspaces(filter, It.IsAny<int>(), null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Name = name,
                ParameterSet = "Name",
            };
            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertExpectedUnitTestResults(expectedWorkspaces, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScopeDoesNotSupportUser()
        {
            // Arrange
            var user = "JOHN@contoso.com";
            var filter = $"users/any(u: tolower(u/emailAddress) eq '{user.ToLower()}')";
            var expectedWorkspaces = new List<Workspace> { new Workspace { Id = Guid.NewGuid(), Name = "TestWorkspace" } };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces.GetWorkspacesAsAdmin(It.IsAny<int>(), "users", filter, null)).Returns(expectedWorkspaces);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                User = user,
            };
            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertGetWorkspacesNeverCalled(client, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScopeDoesNotSupportDeleted()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Deleted = true,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertGetWorkspacesNeverCalled(client, initFactory);
        }

        [TestMethod]
        public void GetWorkspacesIndividualScopeDoesNotSupportOrphaned()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Orphaned = true,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            AssertGetWorkspacesNeverCalled(client, initFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(Rest.HttpOperationException))]
        public void GetWorkspacesIndividualScope_Throttled()
        {
            // Arrange
            var clientHandler = new FakeHttpClientHandler(new System.Net.Http.HttpResponseMessage((System.Net.HttpStatusCode)429));
            var initFactory = new TestPowerBICmdletInitFactory(clientHandler);
            var cmdlet = new GetPowerBIWorkspace(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            Assert.Fail("Should not have reached this point");
        }

        private static void AssertExpectedUnitTestResults(List<Workspace> expectedWorkspaces, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.AreEqual(expectedWorkspaces.Count, results.Count());
            var workspaces = results.Cast<Workspace>().ToList();
            CollectionAssert.AreEqual(expectedWorkspaces, workspaces);
        }

        private static void AssertGetWorkspacesNeverCalled(Mock<IPowerBIApiClient> client, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.IsFalse(results.Any());
            client.Verify(x => x.Workspaces.GetWorkspaces(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
