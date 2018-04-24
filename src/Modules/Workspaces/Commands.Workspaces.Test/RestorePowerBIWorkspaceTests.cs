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
    public class RestorePowerBIWorkspaceTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{RestorePowerBIWorkspace.CmdletVerb}-{RestorePowerBIWorkspace.CmdletName}", typeof(RestorePowerBIWorkspace));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRestoreWorkspaceOrganizationScopePropertiesParameterSet()
        {
            /*
             * Test requires a deleted preview workspace (v2) to exist and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstDeletedWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);
                var updatedName = TestUtilities.GetRandomString();
                var emailAddress = "user1@granularcontrols1.ccsctp.net"; //change to a valid test user account
                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                    { nameof(RestorePowerBIWorkspace.Id), workspace.Id },
                    { nameof(RestorePowerBIWorkspace.RestoredName), updatedName },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), emailAddress },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
                Assert.IsTrue(updatedWorkspace.Users
                    .Any(x => x.UserPrincipalName.Equals(emailAddress, StringComparison.OrdinalIgnoreCase)
                    && x.AccessRight == WorkspaceUserAccessRight.Admin.ToString()));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRestoreWorkspaceOrganizationScopeWorkspaceParameterSet()
        {
            /*
             * Test requires a deleted preview workspace (v2) to exist and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstDeletedWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);
                var updatedName = TestUtilities.GetRandomString();
                var emailAddress = "user1@granularcontrols1.ccsctp.net"; //change to valid test user email
                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                    { nameof(RestorePowerBIWorkspace.RestoredName), updatedName },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), emailAddress },
                    { nameof(RestorePowerBIWorkspace.Workspace), workspace },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
                Assert.IsTrue(updatedWorkspace.Users
                    .Any(x => x.UserPrincipalName.Equals(emailAddress, StringComparison.OrdinalIgnoreCase)
                    && x.AccessRight == WorkspaceUserAccessRight.Admin.ToString()));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRestoreWorkspaceIndividualScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Individual },
                    { nameof(RestorePowerBIWorkspace.Id), new Guid() },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), "user1@contoso.com" },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                try
                {
                    // Act
                    ps.Invoke();

                    Assert.Fail("Should not have reached this point");
                }
                catch (CmdletInvocationException ex)
                {
                    // Assert
                    Assert.AreEqual(ex.InnerException.GetType(), typeof(NotImplementedException));
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void EndToEndRestoreWorkspaceWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                    { nameof(RestorePowerBIWorkspace.Id), new Guid() },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), "user1@contoso.com" },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ParameterBindingException))]
        public void EndToEndRestoreWorkspaceWithoutRequiredParameterIdOrWorkspace()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ps.AddCommand(Cmdlet);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        public void RestorePowerBIWorkspaceOrganizationScope_WorkspaceParameterSet()
        {
            // Arrange
            var workspace = new Workspace { Id = Guid.NewGuid() };
            var restoreRequest = new WorkspaceRestoreRequest { RestoredName = "Undeleted", UserPrincipalName = "john@contoso.com" };
            var expectedResponse = new object();
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Workspaces
                .RestoreDeletedWorkspaceAsAdmin(workspace.Id, It.Is<WorkspaceRestoreRequest>(r => r.RestoredName == restoreRequest.RestoredName && r.UserPrincipalName == restoreRequest.UserPrincipalName)))
                .Returns(expectedResponse);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RestorePowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Workspace = workspace,
                RestoredName = restoreRequest.RestoredName,
                UserPrincipalName = restoreRequest.UserPrincipalName,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }

        [TestMethod]
        public void RestorePowerBIWorkspaceIndividualScope()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RestorePowerBIWorkspace(initFactory)
            {
                Scope = PowerBIUserScope.Individual,
                Id = Guid.NewGuid(),
                UserPrincipalName = "john@contoso.com",
            };

            try
            {
                // Act
                cmdlet.InvokePowerBICmdlet();

                Assert.Fail("Should not have reached this point");
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                // Assert
                Assert.AreEqual(ex.InnerException.GetType(), typeof(NotImplementedException));
            }
        }
    }
}
