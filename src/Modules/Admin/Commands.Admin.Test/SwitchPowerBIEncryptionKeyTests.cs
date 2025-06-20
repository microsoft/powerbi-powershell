﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class SwitchPowerBIEncryptionKeyTests
    {
        private static CmdletInfo SwitchPowerBIEncryptionKeyCmdletInfo => new CmdletInfo($"{SwitchPowerBIEncryptionKey.CmdletVerb}-{SwitchPowerBIEncryptionKey.CmdletName}", typeof(SwitchPowerBIEncryptionKey));

        private static string MockName = "KeyName";
        private static string MockKeyVaultKeyUri = "http://www.contoso.com/";
        private static bool MockDefault = true;

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        [ExcludeFromCodeCoverage]
        public void EndToEndRotatePowerBIEncryptionKey()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SwitchPowerBIEncryptionKeyCmdletInfo)
                        .AddParameter(nameof(SwitchPowerBIEncryptionKey.Name), MockName)
                        .AddParameter(nameof(SwitchPowerBIEncryptionKey.KeyVaultKeyUri), MockKeyVaultKeyUri);

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
            var tenantKey1 = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = "KeyName1",
                KeyVaultKeyIdentifier = new Uri("http://www.contoso1.com/"),
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var tenantKey2 = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = "KeyName2",
                KeyVaultKeyIdentifier = new Uri("http://www.contoso2.com/"),
                IsDefault = false,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var rotatedTenantKey = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = "KeyName1",
                KeyVaultKeyIdentifier = new Uri("http://www.contoso3.com/"),
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var tenantKeys = new List<EncryptionKey>() { tenantKey1, tenantKey2 };
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(tenantKeys);
            client.Setup(x => x.Admin.RotatePowerBIEncryptionKey(It.IsAny<Guid>(), It.IsAny<string>())).Returns(rotatedTenantKey);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SwitchPowerBIEncryptionKey(initFactory)
            {
                Name = "KeyName1",
                KeyVaultKeyUri = "http://www.contoso3.com/"
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.RotatePowerBIEncryptionKey(tenantKey1.Id, "http://www.contoso3.com/"), Times.Once());
            AssertExpectedUnitTestResults(rotatedTenantKey, initFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RotatePowerBIEncryptionKey_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var tenantKey = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = MockName,
                KeyVaultKeyIdentifier = new Uri(MockKeyVaultKeyUri),
                IsDefault = MockDefault,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SwitchPowerBIEncryptionKey(initFactory)
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
            var tenantKey = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = MockName,
                KeyVaultKeyIdentifier = new Uri(MockKeyVaultKeyUri),
                IsDefault = MockDefault,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(new List<EncryptionKey>());
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SwitchPowerBIEncryptionKey(initFactory)
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

        private static void AssertExpectedUnitTestResults(EncryptionKey expectedTenantKey, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            var encryptionKeys = results.Cast<EncryptionKey>().ToList();
            Assert.AreEqual(encryptionKeys.Count, 1);
            Assert.AreEqual(expectedTenantKey, encryptionKeys[0]);
        }
    }
}
