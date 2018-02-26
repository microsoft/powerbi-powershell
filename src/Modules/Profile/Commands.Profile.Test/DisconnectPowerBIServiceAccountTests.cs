using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    [TestClass]
    public class DisconnectPowerBIServiceAccountTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void LogoutNoLoginTest()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(new CmdletInfo($"{DisconnectPowerBIServiceAccount.CmdletVerb}-{DisconnectPowerBIServiceAccount.CmdletName}", typeof(DisconnectPowerBIServiceAccount)));
                var result = ps.Invoke();
            }
        }
    }
}
