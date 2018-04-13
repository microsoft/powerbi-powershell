/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class RemovePowerBIWorkspaceUserTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{RemovePowerBIWorkspaceUser.CmdletVerb}-{RemovePowerBIWorkspaceUser.CmdletName}", typeof(RemovePowerBIWorkspaceUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemovePowerBIWorkspaceUserOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceWithUsersInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);

                var emailAddress = "user1@granularcontrols1.ccsctp.net";
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(RemovePowerBIWorkspaceUser.Scope), PowerBIUserScope.Organization },
                    { nameof(RemovePowerBIWorkspaceUser.Id), workspace.Id },
                    { nameof(RemovePowerBIWorkspaceUser.UserPrincipalName), emailAddress },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.IsFalse(updatedWorkspace.Users.Any(x => x.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase)));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRemovePowerBIWorkspaceUserIndividualScope()
        {
            // TODO: Note that unlike the admin APIs, this API will throw an error when attempting to remove a user that does not have access to the workspace
            // This means that this end-to-end test can fail depending on when it is run with the other tests
            // This can't be elegantly solved until users are available on the non-admin GET endpoint
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);
                WorkspacesTestUtilities.AssertShouldContinueIndividualTest(workspace);

                var emailAddress = "user1@granularcontrols1.ccsctp.net";
                var parameters = new Dictionary<string, object>()
                {
                    { nameof(RemovePowerBIWorkspaceUser.Scope), PowerBIUserScope.Individual },
                    { nameof(RemovePowerBIWorkspaceUser.Id), workspace.Id }, 
                    { nameof(RemovePowerBIWorkspaceUser.UserPrincipalName), emailAddress},
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallRemovePowerBIWorkspaceUserWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                var parameters = new Dictionary<string, object>()
                {
                    { nameof(RemovePowerBIWorkspaceUser.Id), new Guid() },
                    { nameof(RemovePowerBIWorkspaceUser.UserPrincipalName), "user1@granularcontrols1.ccsctp.net" },
                };

                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }
    }
}