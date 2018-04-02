/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class GetPowerBIWorkspaceTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForOrganization()
        {
            /*
             * Test requires a preview Workspace (v2) to exist and login as an administrator
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameter(nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization));
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                if(result.Count == 0)
                {
                    Assert.Inconclusive("No workspaces returned, verify you have workspaces in your organization");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForIndividual()
        {
            /*
             * Test requires at least one workspace (group or preview Workspace)
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                if (result.Count == 0)
                {
                    Assert.Inconclusive("No workspaces returned, verify you are assigned or own any workspaces");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForOrganizationAndFilter()
        {
            /*
             * Test requires a preview Workspace (v2) to exist and login as an administrator.
             * The workspace needs to be named "Preview Workspace" and no workspace exists with the name "Nonexistant Workspace".
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization) },
                        { nameof(GetPowerBIWorkspace.Filter), "name eq 'Preview Workspace'" }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.Filter)] = "name eq 'Nonexistant Workspace'";
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspacesForOrganizationAndUser()
        {
            /*
             * Test requires a preview Workspace (v2) to exist and login as an administrator.
             * The workspace needs to assigned to user User1@granularcontrols1.ccsctp.net and no workspace should be assigned to fakeuser@granularcontrols1.ccsctp.net
             */

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization) },
                        { nameof(GetPowerBIWorkspace.User), "User1@granularcontrols1.ccsctp.net" }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.User)] = "fakeuser@granularcontrols1.ccsctp.net";
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallGetWorkspacesWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace)));
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
