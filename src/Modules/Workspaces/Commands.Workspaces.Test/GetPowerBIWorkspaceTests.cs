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
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace)));
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
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);
                var parameters = new Dictionary<string, object>()
                    {
                        { nameof(GetPowerBIWorkspace.Scope), nameof(PowerBIUserScope.Organization) },
                        { nameof(GetPowerBIWorkspace.Filter), "name eq 'Better Work'" }
                    };
                ps.AddCommand(WorkspacesTestUtilities.GetPowerBIWorkspaceCmdletInfo)
                    .AddParameters(parameters);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                ps.Commands.Clear();

                parameters[nameof(GetPowerBIWorkspace.Filter)] = "name eq 'Not Real'";
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
                try
                {
                    ProfileTestUtilities.DisconnectToPowerBI(ps);
                }
                catch (Exception) { } // ignore, not part of the test

                ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace)));
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
