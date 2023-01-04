/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
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
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void ConnectPowerBIServiceWithDiscoveryUrl()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.DiscoveryUrl), "https://api.powerbi.com/powerbi/globalservice/v201606/environments/discover?client=powerbi-msolap");
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.CustomEnvironment), "GlobalCloud");

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
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void ConnectPowerBIServiceAccountServiceWithTenantId_UserParameterSet()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                var environment = PowerBIEnvironmentType.OneBox;
                var tenant = "039db662-19f0-4ca7-869a-3238540f1dd0"; // Onebox tenant for ComputeCdsa

#if DEBUG
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo)
                    .AddParameter(nameof(ConnectPowerBIServiceAccount.Environment), environment)
                    .AddParameter(nameof(ConnectPowerBIServiceAccount.Tenant), tenant);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsTrue(results.Count == 1);
                Assert.IsTrue(results[0].BaseObject is PowerBIProfile);
                var profile = results[0].BaseObject as PowerBIProfile;
                Assert.AreEqual(tenant, profile.TenantId);

                // Disconnect
                ps.Commands.Clear();
                ps.AddCommand(ProfileTestUtilities.DisconnectPowerBIServiceAccountCmdletInfo);
                ps.Invoke();
#endif
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

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ConnectPowerBIServiceAccountDiscoveryUrl_NullCustomEnvironment()
        {
            // Arrange
            var initFactory = new TestPowerBICmdletNoClientInitFactory(false);
            var cmdlet = new ConnectPowerBIServiceAccount(initFactory)
            {
                DiscoveryUrl = "https://api.powerbi.com/powerbi/globalservice/v201606/environments/discover?client=powerbi-msolap"
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            //Assert
            Assert.Fail("Custom environment was not provided");
        }
    }
}