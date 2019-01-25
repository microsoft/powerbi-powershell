/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Admin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class RotatePowerBIEncryptionKeyTests
    {
        private static CmdletInfo RotatePowerBIEncryptionKeyCmdletInfo => new CmdletInfo($"{RotatePowerBIEncryptionKey.CmdletVerb}-{RotatePowerBIEncryptionKey.CmdletName}", typeof(RotatePowerBIEncryptionKey));

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
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(RotatePowerBIEncryptionKeyCmdletInfo)
                        .AddParameter(nameof(RotatePowerBIEncryptionKey.Name), MockName)
                        .AddParameter(nameof(RotatePowerBIEncryptionKey.KeyVaultKeyUri), MockKeyVaultKeyUri);

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
            var encryptionKey1 = new EncryptionKey(Guid.NewGuid(), "KeyName1", "KeyVaultUri1", true, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            var encryptionKey2 = new EncryptionKey(Guid.NewGuid(), "KeyName2", "KeyVaultUri2", false, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            var rotatedEncryptionKey = new EncryptionKey(encryptionKey1.Id, "KeyName1", "KeyVaultUri3", true, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            var encryptionKeys = new List<EncryptionKey>();
            encryptionKeys.Add(encryptionKey1);
            encryptionKeys.Add(encryptionKey2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(encryptionKeys);
            client.Setup(x => x.Admin.RotatePowerBIEncryptionKey(It.IsAny<string>(), It.IsAny<string>())).Returns(rotatedEncryptionKey);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RotatePowerBIEncryptionKey(initFactory)
            {
                Name = "KeyName1",
                KeyVaultKeyUri = "KeyVaultUri3"
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.RotatePowerBIEncryptionKey(encryptionKey1.Id.ToString(), "KeyVaultUri3"), Times.Once());
            AssertExpectedUnitTestResults(rotatedEncryptionKey, initFactory);
        }

        [TestMethod]
        public void RotatePowerBIEncryptionKey_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var encryptionKey = new EncryptionKey(Guid.NewGuid(), MockName, MockKeyVaultKeyUri, MockDefault, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RotatePowerBIEncryptionKey(initFactory)
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
            var encryptionKey = new EncryptionKey(Guid.NewGuid(), MockName, MockKeyVaultKeyUri, MockDefault, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(new List<EncryptionKey>());
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new RotatePowerBIEncryptionKey(initFactory)
            {
                Name = MockName,
                KeyVaultKeyUri = MockKeyVaultKeyUri
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw Exception");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "No matching encryption keys found");
        }

        private static void AssertExpectedUnitTestResults(EncryptionKey expectedEncryptionKey, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            var encryptionKeys = results.Cast<EncryptionKey>().ToList();
            Assert.AreEqual(encryptionKeys.Count, 1);
            Assert.AreEqual(expectedEncryptionKey, encryptionKeys[0]);
        }
    }
}
