/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class AddPowerBIWorkspaceUserTest
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIWorkspaceUserAsAdmin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);

                var workspace = WorkspacesTestUtilities.GetWorkspace(ps);

                if (workspace == null)
                {
                    Assert.Inconclusive("No workspaces found to perform end to end test");
                }

                var parameters = new Dictionary<string, object>()
                {
                    { "Scope", "Organization"},
                    { "Id", workspace.Id},
                    { "UserEmailAddress", "user1@granularcontrols1.ccsctp.net"}, //update parameters for all tests to use a test account, this user email will only work on Onebox
                    { "UserAccessRight", "Admin" }
                };

                ps.AddCommand(new CmdletInfo($"{AddPowerBIWorkspaceUser.CmdletVerb}-{AddPowerBIWorkspaceUser.CmdletName}", typeof(AddPowerBIWorkspaceUser))).AddParameters(parameters);
                var result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIWorkspaceUserAsIndividual()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);

                var group = WorkspacesTestUtilities.GetGroup(ps);

                if (group == null)
                {
                    Assert.Inconclusive("No groups found to perform end to end test");
                }

                var parameters = new Dictionary<string, object>()
                {
                    { "Scope", "Individual"},
                    { "Id", group.Id }, 
                    { "UserEmailAddress", "user1@granularcontrols1.ccsctp.net"},
                    { "UserAccessRight", "Admin" }
                };

                ps.AddCommand(new CmdletInfo($"{AddPowerBIWorkspaceUser.CmdletVerb}-{AddPowerBIWorkspaceUser.CmdletName}", typeof(AddPowerBIWorkspaceUser))).AddParameters(parameters);
                var result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallAddPowerBIWorkspaceUserWithoutLogin()
        {

            using (var ps = System.Management.Automation.PowerShell.Create())
            { 
                var parameters = new Dictionary<string, object>()
                {
                    { "Scope", "Organization"},
                    { "Id", new Guid()},
                    { "UserEmailAddress", "user1@granularcontrols1.ccsctp.net"},
                    { "UserAccessRight", "Member" }
                };

                ps.AddCommand(new CmdletInfo($"{AddPowerBIWorkspaceUser.CmdletVerb}-{AddPowerBIWorkspaceUser.CmdletName}", typeof(AddPowerBIWorkspaceUser))).AddParameters(parameters);
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
