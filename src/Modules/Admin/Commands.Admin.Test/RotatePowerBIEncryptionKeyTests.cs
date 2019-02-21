/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class RotatePowerBIEncryptionKeyTests
    {
        private static CmdletInfo RotatePowerBIEncryptionKeyCmdletInfo => new CmdletInfo($"{SetPowerBIEncryptionKey.CmdletVerb}-{SetPowerBIEncryptionKey.CmdletName}", typeof(SetPowerBIEncryptionKey));

        private static string MockName = "KeyName";
        private static string MockKeyVaultKeyUri = "KeyVaultUri";
        private static bool MockDefault = true;

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndRotatePowerBIEncryptionKey()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(RotatePowerBIEncryptionKeyCmdletInfo)
                        .AddParameter(nameof(SetPowerBIEncryptionKey.Name), MockName)
                        .AddParameter(nameof(SetPowerBIEncryptionKey.KeyVaultKeyUri), MockKeyVaultKeyUri);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void RotatePowerBIEncryptionKey_WithAllValidParameters()
        {
            // Arrange
            var tenantKey1 = new TenantKey()
            {
                Id = Guid.NewGuid(),
                Name = "KeyName1",
                KeyVaultKeyIdentifier = "KeyVaultUri1",
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var tenantKey2 = new TenantKey()
            {
                Id = Guid.NewGuid(),
                Name = "KeyName2",
                KeyVaultKeyIdentifier = "KeyVaultUri2",
                IsDefault = false,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var rotatedTenantKey = new TenantKey()
            {
                Id = Guid.NewGuid(),
                Name = "KeyName1",
                KeyVaultKeyIdentifier = "KeyVaultUri3",
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var tenantKeys = new List<TenantKey>();
            tenantKeys.Add(tenantKey1);
            tenantKeys.Add(tenantKey2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(tenantKeys);
            client.Setup(x => x.Admin.RotatePowerBIEncryptionKey(It.IsAny<string>(), It.IsAny<string>())).Returns(rotatedTenantKey);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBIEncryptionKey(initFactory)
            {
                Name = "KeyName1",
                KeyVaultKeyUri = "KeyVaultUri3"
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.RotatePowerBIEncryptionKey(tenantKey1.Id.ToString(), "KeyVaultUri3"), Times.Once());
            AssertExpectedUnitTestResults(rotatedTenantKey, initFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RotatePowerBIEncryptionKey_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var tenantKey = new TenantKey()
            {
                Id = Guid.NewGuid(),
                Name = MockName,
                KeyVaultKeyIdentifier = MockKeyVaultKeyUri,
                IsDefault = MockDefault,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw Exception");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Some exception");
        }

        [TestMethod]
        public void RotatePowerBIEncryptionKey_WithNoMatchingEncryptionKeys()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var tenantKey = new TenantKey()
            {
                Id = Guid.NewGuid(),
                Name = MockName,
                KeyVaultKeyIdentifier = MockKeyVaultKeyUri,
                IsDefault = MockDefault,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(new List<TenantKey>());
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw Exception");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "No encryption keys are set");
        }

        private static void AssertExpectedUnitTestResults(TenantKey expectedTenantKey, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            var encryptionKeys = results.Cast<TenantKey>().ToList();
            Assert.AreEqual(encryptionKeys.Count, 1);
            Assert.AreEqual(expectedTenantKey, encryptionKeys[0]);
        }
    }
}
