/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    public static class WorkspacesTestUtilities
    {
        public static CmdletInfo GetPowerBIWorkspaceCmdletInfo => new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace));

        public static void AssertShouldContinueOrganizationTest(Workspace workspace)
        {
            if (workspace == null)
            {
                Assert.Inconclusive("No workspaces returned. Verify you have workspaces in your organization.");
            }
        }

        public static void AssertShouldContinueIndividualTest(Workspace workspace)
        {
            if (workspace == null)
            {
                Assert.Inconclusive("No workspaces returned. Verify you are assigned or own any workspaces.");
            }
        }

        public static Workspace GetWorkspace(System.Management.Automation.PowerShell ps, PowerBIUserScope scope, Guid id)
        {
            return InvokeGetPowerBIWorkspace(ps, scope, OperationType.GetWorkspaceById, id);
        }

        public static Workspace GetFirstWorkspace(System.Management.Automation.PowerShell ps, PowerBIUserScope scope)
        {
            return InvokeGetPowerBIWorkspace(ps, scope, OperationType.GetFirstWorkspace);
        }

        // TODO: Until the non-admin endpoint exposes type, this can only call the cmdlet with Organization scope
        public static Workspace GetFirstWorkspaceInOrganization(System.Management.Automation.PowerShell ps)
        {
            return InvokeGetPowerBIWorkspace(ps, PowerBIUserScope.Organization, OperationType.GetFirstWorkspaceInOrganization);
        }

        // TODO: Until the non-admin endpoint supports users, this can only call the cmdlet with Organization scope
        public static Workspace GetFirstWorkspaceWithUsersInOrganization(System.Management.Automation.PowerShell ps)
        {
            return InvokeGetPowerBIWorkspace(ps, PowerBIUserScope.Organization, OperationType.GetFirstWorkspaceWithAUserInOrganization);
        }

        public static Workspace GetFirstDeletedWorkspaceInOrganization(System.Management.Automation.PowerShell ps)
        {
            return InvokeGetPowerBIWorkspace(ps, PowerBIUserScope.Organization, OperationType.GetFirstDeletedWorkspaceInOrganization);
        }

        private static Workspace InvokeGetPowerBIWorkspace(System.Management.Automation.PowerShell ps, PowerBIUserScope scope, OperationType type, Guid? id = null)
        {
            var first = 5000;
            var skip = 0;
            while (true)
            {
                ps.Commands.Clear();
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), scope.ToString() },
                        { nameof(GetPowerBIWorkspace.First), first },
                        { nameof(GetPowerBIWorkspace.Skip), skip }
                    };
                ps.AddCommand(GetPowerBIWorkspaceCmdletInfo).AddParameters(parameters);
                var results = ps.Invoke();
                if (results.Any())
                {
                    var workspace = GetWorkspaceForOperationType(results, type, id);
                    if (workspace != null)
                    {
                        ps.Commands.Clear();
                        return workspace;
                    }
                }
                if (results.Count < first)
                {
                    break;
                }
                skip += first;
            }
            
            TestUtilities.AssertNoCmdletErrors(ps);
            ps.Commands.Clear();
            return null;
        }

        private static Workspace GetWorkspaceForOperationType(ICollection<PSObject> results, OperationType type, Guid? id = null)
        {
            switch (type)
            {
                case OperationType.GetWorkspaceById:
                    return results.Select(x => (Workspace)x.BaseObject).First(x => x.Id == id);

                case OperationType.GetFirstWorkspace:
                    return results.Select(x => (Workspace)x.BaseObject).First();

                case OperationType.GetFirstWorkspaceInOrganization:
                    return results.Select(x => (Workspace)x.BaseObject).FirstOrDefault(x => x.Type == WorkspaceType.Workspace);

                case OperationType.GetFirstWorkspaceWithAUserInOrganization:
                    return results.Select(x => (Workspace)x.BaseObject).FirstOrDefault(x => x.Type == WorkspaceType.Workspace && x.Users.Any());

                case OperationType.GetFirstDeletedWorkspaceInOrganization:
                    return results.Select(x => (Workspace)x.BaseObject).FirstOrDefault(x => x.Type == WorkspaceType.Workspace && x.State == WorkspaceState.Deleted);

                default:
                    return null;
            }
        }
    }

    enum OperationType
    {
        GetWorkspaceById,
        GetFirstWorkspace,
        GetFirstWorkspaceInOrganization,
        GetFirstWorkspaceWithAUserInOrganization,
        GetFirstDeletedWorkspaceInOrganization
    }
}
