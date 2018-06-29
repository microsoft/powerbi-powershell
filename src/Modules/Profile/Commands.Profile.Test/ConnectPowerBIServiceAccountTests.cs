/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using System.Security;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                // Arrange
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsTrue(results.Count == 1);
                Assert.IsTrue(results[0].BaseObject is PowerBIProfile);
                var profile = results[0].BaseObject as PowerBIProfile;
                Assert.IsNotNull(profile.Environment);
                Assert.IsNotNull(profile.UserName);
                Assert.IsNotNull(profile.TenantId);

                // Arrange
                ps.Commands.Clear();
                ps.AddCommand(ProfileTestUtilities.DisconnectPowerBIServiceAccountCmdletInfo);

                // Act
                results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.AreEqual(0, results.Count);
            }
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccountServiceWithTenantId_PrincipalParameterSet()
        {
            // Arrange
            var initFactory = new TestPowerBICmdletNoClientInitFactory(false);
            var testTenantName = "test.microsoftonline.com";
            var cmdlet = new ConnectPowerBIServiceAccount(initFactory)
            {
                Tenant = testTenantName,
                ServicePrincipal = true,
                Credential = new PSCredential("appId", new SecureString()),
                ParameterSet = "ServicePrincipal"
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var profile = initFactory.GetProfileFromStorage();
            Assert.IsNotNull(profile);
            Assert.IsTrue(profile.Environment.AzureADAuthority.Contains(testTenantName));
            initFactory.AssertExpectedUnitTestResults(new[] { profile });
        }
    }
}
