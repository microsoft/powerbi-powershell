/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Profile;
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
                ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount))).AddParameter("Environment", "PPE");
                var result = ps.Invoke();
                //ps.Streams.Error
                Assert.IsFalse(ps.HadErrors);
                Assert.IsNotNull(result);
                ps.Commands.Clear();

                var workspace = GetWorkspace(ps);
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
                result = ps.Invoke();
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
                ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount))).AddParameter("Environment", "PPE");
                var result = ps.Invoke();
                //ps.Streams.Error
                Assert.IsFalse(ps.HadErrors);
                Assert.IsNotNull(result);
                ps.Commands.Clear();

                var group = GetGroup(ps);
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
                result = ps.Invoke();
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

        //Move this method to a common test utilities file
        private static Group GetWorkspace(System.Management.Automation.PowerShell ps, string id = null)
        {
            ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace))).AddParameter("Scope", "Organization");
            var results = ps.Invoke();
            ps.Commands.Clear();

            if (results.Any())
            {
                var workspaces = results.Select(x => (Group)x.BaseObject);

                return id == null ? workspaces.First() : workspaces.First(x => x.Id == id);
            }

            return null;
        }

        private static Group GetGroup(System.Management.Automation.PowerShell ps, string id = null)
        {
            ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace))).AddParameter("Scope", "Individual");
            var results = ps.Invoke();
            ps.Commands.Clear();

            if (results.Any())
            {
                var workspaces = results.Select(x => (Group)x.BaseObject);

                return id == null ? workspaces.First() : workspaces.First(x => x.Id == id);
            }

            return null;
        }
    }
}
