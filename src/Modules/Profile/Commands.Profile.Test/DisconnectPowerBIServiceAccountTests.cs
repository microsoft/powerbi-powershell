/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Commands.Common.Test;
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
                ps.AddCommand(ProfileTestUtilities.DisconnectPowerBIServiceAccountCmdletInfo);
                var result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }
    }
}
