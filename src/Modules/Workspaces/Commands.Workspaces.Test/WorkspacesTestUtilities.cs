/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Linq;
using System.Management.Automation;
using Commands.Common.Test;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    public static class WorkspacesTestUtilities
    {
        public static Group GetWorkspace(System.Management.Automation.PowerShell ps, string id = null)
        {
            ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace))).AddParameter("Scope", "Organization");
            var results = ps.Invoke();
            TestUtilities.AssertNoCmdletErrors(ps);
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
