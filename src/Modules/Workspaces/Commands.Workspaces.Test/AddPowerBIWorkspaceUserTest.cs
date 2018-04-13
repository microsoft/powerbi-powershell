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
    public class AddPowerBIWorkspaceUserTest
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{AddPowerBIWorkspaceUser.CmdletVerb}-{AddPowerBIWorkspaceUser.CmdletName}", typeof(AddPowerBIWorkspaceUser));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIWorkspaceUserOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);

                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Scope), PowerBIUserScope.Organization },
                    { nameof(AddPowerBIWorkspaceUser.Id), workspace.Id},
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), "user1@granularcontrols1.ccsctp.net"}, //update parameters for all tests to use a test account, this user email will only work on OneBox
                    { nameof(AddPowerBIWorkspaceUser.UserAccessRight), GroupUserAccessCmdletEnum.Admin }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspace(ps, PowerBIUserScope.Individual);
                WorkspacesTestUtilities.AssertShouldContinueIndividualTest(workspace);

                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Scope), PowerBIUserScope.Individual },
                    { nameof(AddPowerBIWorkspaceUser.Id), workspace.Id }, 
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), "user1@granularcontrols1.ccsctp.net" },
                    { nameof(AddPowerBIWorkspaceUser.UserAccessRight), GroupUserAccessCmdletEnum.Admin }
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
        public void CallAddPowerBIWorkspaceUserWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                var parameters = new Dictionary<string, object>()
                {
                    { nameof(AddPowerBIWorkspaceUser.Id), new Guid() },
                    { nameof(AddPowerBIWorkspaceUser.UserPrincipalName), "user1@granularcontrols1.ccsctp.net" },
                    { nameof(AddPowerBIWorkspaceUser.UserAccessRight), GroupUserAccessCmdletEnum.Member }
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
