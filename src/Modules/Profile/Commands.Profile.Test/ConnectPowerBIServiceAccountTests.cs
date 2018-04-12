/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commands.Common.Test;

namespace Microsoft.PowerBI.Commands.Profile.Test
{
    [TestClass]
    public class ConnectPowerBIServiceAccountTests
    {
        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndInteractiveLogin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);
                var result = ps.Invoke();
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsTrue(result.Count == 1);
                Assert.IsTrue(result[0].BaseObject is PowerBIProfile);
                var profile = result[0].BaseObject as PowerBIProfile;
                Assert.IsNotNull(profile.Environment);
                Assert.IsNotNull(profile.UserName);
                Assert.IsNotNull(profile.TenantId);

                ps.Commands.Clear();
                ps.AddCommand(ProfileTestUtilities.DisconnectPowerBIServiceAccountCmdletInfo);
                result = ps.Invoke();
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count);
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }
    }
}
