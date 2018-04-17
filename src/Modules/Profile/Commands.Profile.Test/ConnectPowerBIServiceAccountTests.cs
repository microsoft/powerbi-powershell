/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.PowerBI.Commands.Common.Test;

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

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsTrue(results.Count == 1);
                Assert.IsTrue(results[0].BaseObject is PowerBIProfile);
                var profile = results[0].BaseObject as PowerBIProfile;
                Assert.IsNotNull(profile.Environment);
                Assert.IsNotNull(profile.UserName);
                Assert.IsNotNull(profile.TenantId);

                ps.Commands.Clear();
                ps.AddCommand(ProfileTestUtilities.DisconnectPowerBIServiceAccountCmdletInfo);

                results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.AreEqual(0, results.Count);
            }
        }
    }
}
