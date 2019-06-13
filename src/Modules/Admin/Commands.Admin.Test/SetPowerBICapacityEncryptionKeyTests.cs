/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Capacities;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class SetPowerBICapacityEncryptionKeyTests
    {
        private static CmdletInfo SetPowerBICapacityEncryptionKeyCmdletInfo => new CmdletInfo($"{SetPowerBICapacityEncryptionKey.CmdletVerb}-{SetPowerBICapacityEncryptionKey.CmdletName}", typeof(SetPowerBICapacityEncryptionKey));

        private static string MockKeyName = "KeyName";
        private static Guid MockCapacityId = Guid.NewGuid();

        [TestMethod]
        public void SetPowerBICapacityEncryptionKey_KeyNameAndCapacityIdParameterSet()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var encryptionKeys = SetupPowerBIEncryptionKeyMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                KeyName = encryptionKeys[0].Name,
                CapacityId = MockCapacityId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(encryptionKeys[0].Id, MockCapacityId), Times.Once());
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
        }

        [TestMethod]
        public void SetPowerBICapacityEncryptionKey_WithKeyNameAndCapacityParameterSet()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var encryptionKeys = SetupPowerBIEncryptionKeyMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                KeyName = encryptionKeys[0].Name,
                Capacity = new Capacity() { Id = MockCapacityId },
                ParameterSet = SetPowerBICapacityEncryptionKey.KeyNameAndCapacityParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(encryptionKeys[0].Id, MockCapacityId), Times.Once());
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
        }

        [TestMethod]
        public void SetPowerBICapacityEncryptionKey_WithKeyAndCapacityIdParameterSet()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var encryptionKeys = SetupPowerBIEncryptionKeyMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                Key = encryptionKeys[0],
                CapacityId = MockCapacityId,
                ParameterSet = SetPowerBICapacityEncryptionKey.KeyAndCapacityIdParameterSet
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(encryptionKeys[0].Id, MockCapacityId), Times.Once());
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SetPowerBICapacityEncryptionKey_WithGetEncryptionKeyApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                KeyName = MockKeyName,
                CapacityId = MockCapacityId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Some exception");
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SetPowerBICapacityEncryptionKey_WithSetEncryptionKeyApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var encryptionKeys = SetupPowerBIEncryptionKeyMock(client);
            client.Setup(x => x.Admin.SetPowerBICapacityEncryptionKey(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                KeyName = encryptionKeys[0].Name,
                CapacityId = MockCapacityId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Some exception");
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(encryptionKeys[0].Id, MockCapacityId), Times.Once());
        }

        [TestMethod]
        public void SetPowerBICapacityEncryptionKey_WithNoMatchingEncryptionKeys()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var encryptionKeys = SetupPowerBIEncryptionKeyMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                KeyName = "RandomKeyName",
                CapacityId = MockCapacityId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "No matching encryption keys found");
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void SetPowerBICapacityEncryptionKey_WithNoEncryptionKeySet()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(new List<EncryptionKey>());
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetPowerBICapacityEncryptionKey(initFactory)
            {
                KeyName = "RandomKeyName",
                CapacityId = MockCapacityId
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "No encryption keys are set");
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
            client.Verify(x => x.Admin.SetPowerBICapacityEncryptionKey(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }

        private List<EncryptionKey> SetupPowerBIEncryptionKeyMock(Mock<IPowerBIApiClient> client)
        {
            var encryptionKey1 = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = MockKeyName + "-1",
                KeyVaultKeyIdentifier = new Uri("http://www.contoso1.com/"),
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var encryptionKey2 = new EncryptionKey()
            {
                Id = Guid.NewGuid(),
                Name = MockKeyName + "-2",
                KeyVaultKeyIdentifier = new Uri("http://www.contoso2.com/"),
                IsDefault = false,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var encryptionKeys = new List<EncryptionKey>() { encryptionKey1, encryptionKey2 };
            
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(encryptionKeys);
            client.Setup(x => x.Admin.SetPowerBICapacityEncryptionKey(encryptionKey1.Id, MockCapacityId));

            return encryptionKeys;
        }
    }
}
