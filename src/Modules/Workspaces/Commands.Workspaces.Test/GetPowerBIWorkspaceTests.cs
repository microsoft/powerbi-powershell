/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Workspaces.Test
{
    [TestClass]
    public class GetPowerBIWorkspaceTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetWorkspaces()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount))).AddParameter("Environment", "PPE");
                var result = ps.Invoke();
                //ps.Streams.Error
                Assert.IsFalse(ps.HadErrors);
                Assert.IsNotNull(result);
                ps.Commands.Clear();
                ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace))).AddParameter("Scope", "Organization");
                result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallGetWorkspacesWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(new CmdletInfo($"{GetPowerBIWorkspace.CmdletVerb}-{GetPowerBIWorkspace.CmdletName}", typeof(GetPowerBIWorkspace)));
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
