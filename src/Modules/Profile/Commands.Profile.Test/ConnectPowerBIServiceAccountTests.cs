/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Security;
using Microsoft.IdentityModel.Tokens;
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public void ConnectPowerBIServiceAccountServiceWithTenantId_UserParameterSet()
        {
            PowerBIEnvironmentType? environment = null;
            string tenant = null;

            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                if (environment == null)
                {
#if DEBUG
                    environment = PowerBIEnvironmentType.OneBox;
                    // onebox tenant for computeCdsa
                    tenant = "039db662-19f0-4ca7-869a-3238540f1dd0";
#else
                    environment = PowerBIEnvironmentType.Public;
#endif
                }

                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo)
                    .AddParameter(nameof(ConnectPowerBIServiceAccount.Environment), environment)
                    .AddParameter(nameof(ConnectPowerBIServiceAccount.Tenant), tenant);

                var results = ps.Invoke();

                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                Assert.IsTrue(results.Count == 1);
                Assert.IsTrue(results[0].BaseObject is PowerBIProfile);
                var profile = results[0].BaseObject as PowerBIProfile;
                Assert.IsNotNull(profile.Environment);
                Assert.IsNotNull(profile.UserName);
                if (tenant != null)
                {
                    Assert.AreEqual(tenant, profile.TenantId);
                }

                // Disconnect
                ps.Commands.Clear();
                ps.AddCommand(ProfileTestUtilities.DisconnectPowerBIServiceAccountCmdletInfo);
                ps.Invoke();
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
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
        public void ConnectPowerBIServiceAccountServiceWithTenantId_PrincipalParameterSet()
        {
            // Arrange
            using var secureString = new SecureString();

            var initFactory = new TestPowerBICmdletNoClientInitFactory(false);
            var testTenantName = "test.microsoftonline.com";
            var cmdlet = new ConnectPowerBIServiceAccount(initFactory)
            {
                Tenant = testTenantName,
                ServicePrincipal = true,
                Credential = new PSCredential("appId", secureString),
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

        [TestMethod]
        public void ConnectPowerBIServiceAccount_Illegal_Token_Format_Throws()
        {
            // Arrange
            var factory = new TestPowerBICmdletNoClientInitFactory(setProfile: false);

            var cmdlet = new ConnectPowerBIServiceAccount(factory)
            {
                Token = "thisjwtissowrong",
                ParameterSet = ConnectPowerBIServiceAccount.BringYourOwnTokenParameterSet,
            };

            // Act & Assert
            Assert.ThrowsException<SecurityTokenMalformedException>(cmdlet.InvokePowerBICmdlet);
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccount_Expired_Token_Throws()
        {
            // Arrange
            var factory = new TestPowerBICmdletNoClientInitFactory(setProfile: false);

            var cmdlet = new ConnectPowerBIServiceAccount(factory)
            {
                // this is a dummy generated expired token
                Token = "eyJhbGciOiJIUzI1NiJ9.eyJ0ZXN0X2NsYWltIjp0cnVlLC"
                    + "Jpc3MiOiJ1cm46ZXhhbXBsZTppc3N1ZXIiLCJhdWQiOiJ1cm46Z"
                    + "XhhbXBsZTphdWRpZW5jZSIsImV4cCI6MTc3MDcxNzUxNCwiaWF0I"
                    + "joxNzcwNzE3NDU0fQ.8aM6WbtJkp8mOpWKsmFngPFIMdzGIQje1ZhKpHH_UVE",

                ParameterSet = ConnectPowerBIServiceAccount.BringYourOwnTokenParameterSet,
            };

            // Act & Assert
            Assert.ThrowsException<SecurityTokenExpiredException>(cmdlet.InvokePowerBICmdlet);
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccount_Valid_Token_Success()
        {
            // Arrange
            // dummy generated token that will expire in 30 years (from 10022026)
            var dummyToken = "eyJhbGciOiJIUzI1NiJ9.eyJ0ZXN0X2NsYWltIjp0cnVlLCJpc3MiOiJ1cm46ZXhhbXBsZT"
                     + "ppc3N1ZXIiLCJhdWQiOiJ1cm46ZXhhbXBsZTphdWRpZW5jZSIsImV4cCI6MjcxN"
                     + "zQ0Njc4NSwiaWF0IjoxNzcwNzE4Nzg1fQ.nOXgwieIpeFB9Svxxt6Z4_RkWVWSiVJcxbBzlPTaQJQ";

            var factory = new TestPowerBICmdletNoClientInitFactory(setProfile: false);

            var cmdlet = new ConnectPowerBIServiceAccount(factory)
            {
                Token = dummyToken,
                Environment = PowerBIEnvironmentType.Public,
                ParameterSet = ConnectPowerBIServiceAccount.BringYourOwnTokenParameterSet,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var profile = factory.GetProfileFromStorage();
            Assert.IsNotNull(profile);
            Assert.AreEqual(dummyToken, profile.AccessToken);
            Assert.AreEqual(PowerBIEnvironmentType.Public, profile.Environment.Name);
            Assert.AreEqual(PowerBIProfileType.BringYourOwnToken, profile.LoginType);
            factory.AssertExpectedUnitTestResults([profile]);
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccount_BringYourOwnTokenParameterSet_Token_with_CertificateThumbPrint_Throws()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.Token), "dummytoken");
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.CertificateThumbprint), "dummycreds");

                // Act & Assert
                Assert.ThrowsException<ParameterBindingException>(ps.Invoke);
            }
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccount_BringYourOwnTokenParameterSet_Token_with_ApplicationId_Throws()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.Token), "dummytoken");
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.ApplicationId), "applicationId");

                // Act & Assert
                Assert.ThrowsException<ParameterBindingException>(ps.Invoke);
            }
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccount_BringYourOwnTokenParameterSet_Token_with_Credential_Throws()
        {
            using (var secureString = new SecureString())
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.Token), "dummytoken");
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.Credential), new PSCredential("password", secureString));

                // Act & Assert
                Assert.ThrowsException<ParameterBindingException>(ps.Invoke);
            }
        }

        [TestMethod]
        public void ConnectPowerBIServiceAccount_BringYourOwnTokenParameterSet_Token_with_ServicePrincipal_Throws()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ps.AddCommand(ProfileTestUtilities.ConnectPowerBIServiceAccountCmdletInfo);
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.Token), "dummytoken");
                ps.AddParameter(nameof(ConnectPowerBIServiceAccount.ServicePrincipal), new SwitchParameter());

                // Act & Assert
                Assert.ThrowsException<ParameterBindingException>(ps.Invoke);
            }
        }
    }
}