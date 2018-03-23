/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.Management.Automation;
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

                var workspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization );

                if (workspace == null)
                {
                    Assert.Inconclusive("No workspaces found to perform end to end test");
                }

                var parameters = new Dictionary<string, object>()
                {
                    { "Scope", "Organization"},
                    { "Id", workspace.Id},
                    { "UserPrincipalName", "user1@granularcontrols1.ccsctp.net"}, //update parameters for all tests to use a test account, this user email will only work on Onebox
                    { "UserAccessRight", "Admin" }
                };

                ps.AddCommand(Cmdlet).AddParameters(parameters);
                var result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIWorkspaceUserIndividualScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);

                var group = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Individual);

                if (group == null)
                {
                    Assert.Inconclusive("No groups found to perform end to end test");
                }

                var parameters = new Dictionary<string, object>()
                {
                    { "Scope", "Individual"},
                    { "Id", group.Id }, 
                    { "UserPrincipalName", "user1@granularcontrols1.ccsctp.net"},
                    { "UserAccessRight", "Admin" }
                };

                ps.AddCommand(Cmdlet).AddParameters(parameters);
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
                    { "UserPrincipalName", "user1@granularcontrols1.ccsctp.net"},
                    { "UserAccessRight", "Member" }
                };

                ps.AddCommand(Cmdlet).AddParameters(parameters);
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
