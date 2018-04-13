/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameter(nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization);

                var results = ps.Invoke();

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
        public void EndToEndGetWorkspacesIndividualScope()
        {
            /*
             * Test requires at least one workspace (group or preview workspace)
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameter(nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Individual);

                var results = ps.Invoke();

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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.First), 1 }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                var results = ps.Invoke();

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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Individual },
                        { nameof(GetPowerBIWorkspace.First), 1 }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                var results = ps.Invoke();

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

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.Filter)] = string.Format("name eq '{0}'", TestUtilities.GetRandomString());
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                results = ps.Invoke();

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

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.Filter)] = string.Format("name eq '{0}'", TestUtilities.GetRandomString());
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                results = ps.Invoke();

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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceWithUsersInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);

                var userEmailAddress = workspace.Users.FirstOrDefault().EmailAddress;
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                        { nameof(GetPowerBIWorkspace.User), userEmailAddress }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Any());
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.User)] = TestUtilities.GetRandomString();
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);

                results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsFalse(results.Any());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallGetWorkspacesWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo);

                var results = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
