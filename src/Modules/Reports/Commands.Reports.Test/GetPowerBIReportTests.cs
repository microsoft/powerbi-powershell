/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Profile;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Commands.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commands.Reports.Test
{
    [TestClass]
    public class GetPowerBIReportTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetReports()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(new CmdletInfo($"{ConnectPowerBIServiceAccount.CmdletVerb}-{ConnectPowerBIServiceAccount.CmdletName}", typeof(ConnectPowerBIServiceAccount)));
                var result = ps.Invoke();
                Assert.IsNotNull(result);
                ps.Commands.Clear();
                ps.AddCommand(new CmdletInfo($"{GetPowerBIReport.CmdletVerb}-{GetPowerBIReport.CmdletName}", typeof(GetPowerBIReport)));
                result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallGetReportsWithoutLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                ps.AddCommand(new CmdletInfo($"{GetPowerBIReport.CmdletVerb}-{GetPowerBIReport.CmdletName}", typeof(GetPowerBIReport)));
                var result = ps.Invoke();
                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
