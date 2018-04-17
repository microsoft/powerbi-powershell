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
    public class RestorePowerBIWorkspaceTest
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
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstDeletedWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);

                var updatedName = TestUtilities.GetRandomString();
                var emailAddress = "user1@granularcontrols1.ccsctp.net"; //change to a valid test user account
                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                    { nameof(RestorePowerBIWorkspace.Id), workspace.Id },
                    { nameof(RestorePowerBIWorkspace.Name), updatedName },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), emailAddress },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
                Assert.IsTrue(updatedWorkspace.Users
                    .Any(x => x.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase) && x.GroupUserAccessRightProperty == "Admin"));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetWorkspaceOrganizationScopeWorkspaceParameterSet()
        {
            /*
             * Test requires a deleted preview workspace (v2) to exist and login as an administrator
             */
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var workspace = WorkspacesTestUtilities.GetFirstDeletedWorkspaceInOrganization(ps);
                WorkspacesTestUtilities.AssertShouldContinueOrganizationTest(workspace);

                var updatedName = TestUtilities.GetRandomString();
                var emailAddress = "user1@granularcontrols1.ccsctp.net"; //change to valid test user email
                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                    { nameof(RestorePowerBIWorkspace.Name), updatedName },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), emailAddress },
                    { nameof(RestorePowerBIWorkspace.Workspace), workspace },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
                Assert.IsTrue(updatedWorkspace.Users
                    .Any(x => x.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase) && x.GroupUserAccessRightProperty == "Admin"));
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetWorkspaceIndividualScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
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
                    ps.Invoke();

                    Assert.Fail("Should not have reached this point");
                }
                catch (CmdletInvocationException ex)
                {
                    Assert.AreEqual(ex.InnerException.GetType(), typeof(NotImplementedException));
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallRestoreWorkspaceWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                var parameters = new Dictionary<string, object>
                {
                    { nameof(RestorePowerBIWorkspace.Scope), PowerBIUserScope.Organization },
                    { nameof(RestorePowerBIWorkspace.Id), new Guid() },
                    { nameof(RestorePowerBIWorkspace.UserPrincipalName), "user1@contoso.com" },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ParameterBindingException))]
        public void CallSetWorkspaceWithoutRequiredParameterIdOrWorkspace()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(Cmdlet);

                var results = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
