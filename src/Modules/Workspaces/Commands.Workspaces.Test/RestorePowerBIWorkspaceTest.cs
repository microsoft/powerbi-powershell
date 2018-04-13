/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Commands.Common.Test;
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
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);

                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceInOrganization(ps);
                if (workspace == null)
                {
                    Assert.Inconclusive("No workspaces found to perform end to end test");
                }

                var updatedName = TestUtilities.GetRandomString();
                var emailAddress = "user1@granularcontrols1.ccsctp.net"; //change to a valid test user account
                var parameters = new Dictionary<string, object>
                {
                    { "Scope", PowerBIUserScope.Organization },
                    { "Id", workspace.Id },
                    { "Name", updatedName },
                    { "EmailAddress", emailAddress },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var result = ps.Invoke();
                ps.Commands.Clear();

                Assert.IsNotNull(result);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetWorkspaceOrganizationScopeWorkspaceParameterSet()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);

                var workspace = WorkspacesTestUtilities.GetFirstWorkspaceInOrganization(ps);
                if (workspace == null)
                {
                    Assert.Inconclusive("No workspaces found to perform end to end test");
                }

                var updatedName = TestUtilities.GetRandomString();
                var emailAddress = "user1@granularcontrols1.ccsctp.net"; //change to valid test user email
                var parameters = new Dictionary<string, object>
                {
                    { "Scope", PowerBIUserScope.Organization },
                    { "Name", updatedName },
                    { "EmailAddress", emailAddress },
                    { "Workspace", workspace },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var result = ps.Invoke();
                ps.Commands.Clear();

                Assert.IsNotNull(result);
                var updatedWorkspace = WorkspacesTestUtilities.GetWorkspace(ps, PowerBIUserScope.Organization, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
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
                    { "Scope", PowerBIUserScope.Individual },
                    { "Id", new Guid() },
                    { "Name", "Updated Workspace Name" },
                    { "EmailAddress", "user1@granularcontrols1.ccsctp.net" },
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

                var parameters = new Dictionary<string, object> {
                    { "Id", new Guid() },
                    { "EmailAddress", "user1@contoso.com" }
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

                var result = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
