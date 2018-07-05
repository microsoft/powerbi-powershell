/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    [TestClass]
    public class InvokePowerBIRestMethodTests
    {
        public static CmdletInfo InvokePowerBIRestMethodCmdletInfo { get; } = new CmdletInfo($"{InvokePowerBIRestMethod.CmdletVerb}-{InvokePowerBIRestMethod.CmdletName}", typeof(InvokePowerBIRestMethod));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndInvokePowerBIRestMethodWithOutFile()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBI.Common.Abstractions.PowerBIEnvironmentType.Public);

                ps.AddCommand(InvokePowerBIRestMethodCmdletInfo)
                    .AddParameter("Url", "reports/9dd4146f-3236-4c5c-8141-64fa9f6f0f6c/export")
                    .AddParameter("Method", "Get")
                    .AddParameter("OutFile", ".\\test.pbix");

                ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }
    }
}
