/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Commands.Common.Test;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    public static class WorkspacesTestUtilities
    {
        public static CmdletInfo GetPowerBIWorkspaceCmdletInfo => new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace));

        public static void AssertShouldContinueOrganizationTest(Group workspace)
        {
            if (workspace == null)
            {
                Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
            }
        }

        public static void AssertShouldContinueIndividualTest(Group workspace)
        {
            if (workspace == null)
            {
                Assert.Inconclusive("No workspaces returned. Verify you are assigned or own any workspaces.");
            }
        }

        public static Group GetWorkspace(System.Management.Automation.PowerShell ps, PowerBIUserScope scope, string id)
        {
            var results = InvokeGetPowerBIWorkspace(ps, scope);
            if (results.Any())
            {
                var workspaces = results.Select(x => (Group)x.BaseObject);
                return workspaces.First(x => x.Id == id);
            }

            return null;
        }

        public static Group GetFirstWorkspace(System.Management.Automation.PowerShell ps, PowerBIUserScope scope)
        {
            var results = InvokeGetPowerBIWorkspace(ps, scope);
            if (results.Any())
            {
                var workspaces = results.Select(x => (Group)x.BaseObject);
                return workspaces.First();
            }

            return null;
        }

        // TODO: Until the non-admin endpoint exposes type, this can only call the cmdlet with Organization scope
        public static Group GetFirstWorkspaceInOrganization(System.Management.Automation.PowerShell ps)
        {
            var results = InvokeGetPowerBIWorkspace(ps, PowerBIUserScope.Organization);
            if (results.Any())
            {
                var workspaces = results.Select(x => (Group)x.BaseObject);
                return workspaces.FirstOrDefault(x => x.Type == WorkspaceType.Workspace);
            }

            return null;
        }

        // TODO: Until the non-admin endpoint supports users, this can only call the cmdlet with Organization scope
        public static Group GetFirstWorkspaceWithUsersInOrganization(System.Management.Automation.PowerShell ps)
        {
            var results = InvokeGetPowerBIWorkspace(ps, PowerBIUserScope.Organization);
            if (results.Any())
            {
                var workspaces = results.Select(x => (Group)x.BaseObject);
                return workspaces.FirstOrDefault(x => x.Type == WorkspaceType.Workspace && x.Users.Any());
            }

            return null;
        }

        private static ICollection<PSObject> InvokeGetPowerBIWorkspace(System.Management.Automation.PowerShell ps, PowerBIUserScope scope)
        {
            ps.Commands.Clear();
            ps.AddCommand(GetPowerBIWorkspaceCmdletInfo).AddParameter(nameof(GetPowerBIWorkspace.Scope), scope.ToString());
            var results = ps.Invoke();
            TestUtilities.AssertNoCmdletErrors(ps);
            ps.Commands.Clear();
            return results;
        }
    }
}
