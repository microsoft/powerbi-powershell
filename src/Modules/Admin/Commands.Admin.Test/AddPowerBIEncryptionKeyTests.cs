/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
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
    public class AddPowerBIEncryptionKeyTests
    {
        private static CmdletInfo AddPowerBIEncryptionKeyCmdletInfo => new CmdletInfo($"{AddPowerBIEncryptionKey.CmdletVerb}-{AddPowerBIEncryptionKey.CmdletName}", typeof(AddPowerBIEncryptionKey));

        private static string MockName = "KeyName";
        private static string MockKeyVaultKeyUri = "KeyVaultUri";
        private static bool MockDefault = true;
        private static bool MockActivate = true;

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddPowerBIEncryptionKey()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(AddPowerBIEncryptionKeyCmdletInfo)
                        .AddParameter(nameof(AddPowerBIEncryptionKey.Name), MockName)
                        .AddParameter(nameof(AddPowerBIEncryptionKey.KeyVaultKeyUri), MockKeyVaultKeyUri)
                        .AddParameter(nameof(AddPowerBIEncryptionKey.Default), MockDefault)
                        .AddParameter(nameof(AddPowerBIEncryptionKey.Activate), MockActivate);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void AddPowerBIEncryptionKey_WithAllValidParameters()
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
            client.Setup(x => x.Admin.AddPowerBIEncryptionKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(tenantKey);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri,
                Default = MockDefault,
                Activate = MockActivate
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.AddPowerBIEncryptionKey(MockName, MockKeyVaultKeyUri, MockDefault, MockActivate), Times.Once());
        }

        [TestMethod]
        public void AddPowerBIEncryptionKey_WithDefaultParameterSet()
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
            client.Setup(x => x.Admin.AddPowerBIEncryptionKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(tenantKey);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri,
                Default = MockDefault
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw NotSupportedException");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Parameter Activate with value False is not supported");
        }

        [TestMethod]
        public void AddPowerBIEncryptionKey_WithActivateParameterSet()
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
            client.Setup(x => x.Admin.AddPowerBIEncryptionKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(tenantKey);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri,
                Activate = MockActivate
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw NotSupportedException");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Parameter Default with value False is not supported");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddPowerBIEncryptionKey_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.AddPowerBIEncryptionKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddPowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri,
                Default = MockDefault,
                Activate = MockActivate
            };

            // Act
            cmdlet.InvokePowerBICmdlet();
        }
    }
}
