/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Commands.Common.Test;
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

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.AreEqual(0, results.Count);
            }
        }
    }
}
