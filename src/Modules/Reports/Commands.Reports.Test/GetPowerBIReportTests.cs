/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Commands.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commands.Reports.Test
{
    [TestClass]
    public class GetPowerBIReportTests
    {
        private static CmdletInfo Cmdlet => new CmdletInfo($"{GetPowerBIReport.CmdletVerb}-{GetPowerBIReport.CmdletName}", typeof(GetPowerBIReport));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetReportsIndividualScope()
        {
            using (var ps = PowerShell.Create())
            {
                ProfileTestUtilities.ConnectToPowerBI(ps);

                ps.AddCommand(Cmdlet);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No reports returned. Verify you are assigned or own any reports.");
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CmdletInvocationException))]
        public void CallGetReportsWithoutLogin()
        {
            using (var ps = PowerShell.Create())
            {
                ProfileTestUtilities.SafeDisconnectFromPowerBI(ps);

                ps.AddCommand(Cmdlet);

                var results = ps.Invoke();

                Assert.Fail("Should not have reached this point");
            }
        }
    }
}
