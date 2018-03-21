/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Commands.Common.Test;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class SetPowerBIWorkspaceTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{SetPowerBIWorkspace.CmdletVerb}-{SetPowerBIWorkspace.CmdletName}", typeof(SetPowerBIWorkspace));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetWorkspaceOrganizationScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                TestUtilities.ConnectToPowerBI(ps);

                var workspace = GetWorkspace(ps);
                if (workspace == null)
                {
                    return;
                }

                var updatedName = TestUtilities.GetRandomString();
                var updatedDescription = TestUtilities.GetRandomString();
                var parameters = new Dictionary<string, object>
                {
                    { "Scope", PowerBIUserScope.Organization },
                    { "Id", workspace.Id },
                    { "Name", updatedName },
                    { "Description", updatedDescription },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                var result = ps.Invoke();
                ps.Commands.Clear();

                Assert.IsNotNull(result);
                var updatedWorkspace = GetWorkspace(ps, workspace.Id);
                Assert.AreEqual(updatedName, updatedWorkspace.Name);
                Assert.AreEqual(updatedDescription, updatedWorkspace.Description);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetWorkspaceIndividualScope()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                TestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>
                {
                    { "Scope", PowerBIUserScope.Individual },
                    { "Id", new Guid() },
                    { "Name", "Updated Workspace Name" },
                    { "Description", "Updated Workspace Description" },
                };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                try
                {
                    ps.Invoke();
                }
                catch (CmdletInvocationException ex)
                {
                    Assert.AreEqual(ex.InnerException.GetType(), typeof(NotImplementedException));
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallSetWorkspaceWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                var parameters = new Dictionary<string, object> { { "Id", new Guid() } };
                ps.AddCommand(Cmdlet).AddParameters(parameters);

                ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ParameterBindingException))]
        public void CallSetWorkspaceWithoutRequiredParameterId()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(Cmdlet);

                var result = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }

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
    }
}
