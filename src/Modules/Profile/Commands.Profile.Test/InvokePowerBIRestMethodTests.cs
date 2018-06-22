using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
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
        public void TestOutFile()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps, "Public");

                ps.AddCommand(InvokePowerBIRestMethodCmdletInfo)
                    .AddParameter("Url", "reports/9dd4146f-3236-4c5c-8141-64fa9f6f0f6c/export")
                    .AddParameter("Method", "Get")
                    .AddParameter("OutFile", ".\\test.pbix");

                ps.Invoke();
            }
        }
    }
}
