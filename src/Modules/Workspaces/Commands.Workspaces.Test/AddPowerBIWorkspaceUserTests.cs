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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class AddPowerBIWorkspaceUserTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{AddPowerBIWorkspaceUser.CmdletVerb}-{AddPowerBIWorkspaceUser.CmdletName}", typeof(AddPowerBIWorkspaceUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
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
                    { nameof(AddPowerBIWorkspaceUser.UserAccessRight), WorkspaceUserAccessRight.Member }
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
                    { nameof(AddPowerBIWorkspaceUser.UserAccessRight), WorkspaceUserAccessRight.Admin }
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
        public void CallAddPowerBIWorkspaceUserWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Id), new Guid() },
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), "user1@granularcontrols1.ccsctp.net" },
                    { nameof(AddPowerBIWorkspaceUser.UserAccessRight), WorkspaceUserAccessRight.Admin }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                // Act
                var results = ps.Invoke();

                // Assert
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
