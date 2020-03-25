/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class GetPowerBIEncryptionKeyTests
    {
        private static CmdletInfo GetPowerBIEncryptionKeyCmdletInfo => new CmdletInfo($"{GetPowerBIEncryptionKey.CmdletVerb}-{GetPowerBIEncryptionKey.CmdletName}", typeof(GetPowerBIEncryptionKey));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIEncryptionKey()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIEncryptionKeyCmdletInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void GetPowerBIEncryptionKey_WithValidResponse()
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
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var tenantKeys = new List<EncryptionKey>();
            tenantKeys.Add(tenantKey1);
            tenantKeys.Add(tenantKey2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Encryption.GetPowerBIEncryptionKeys()).Returns(tenantKeys);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIEncryptionKey(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Encryption.GetPowerBIEncryptionKeys(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetPowerBIEncryptionKey_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Encryption.GetPowerBIEncryptionKeys()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIEncryptionKey(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIEncryptionKeyAsAdmin()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBIEncryptionKeyCmdletInfo)
                        .AddParameter(nameof(AddPowerBIEncryptionKey.Scope), PowerBIUserScope.Organization);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void GetPowerBIEncryptionKey_AdminWithValidResponse()
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
                IsDefault = true,
                CreatedAt = new DateTime(1995, 1, 1),
                UpdatedAt = new DateTime(1995, 1, 1)
            };
            var tenantKeys = new List<EncryptionKey>();
            tenantKeys.Add(tenantKey1);
            tenantKeys.Add(tenantKey2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(tenantKeys);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIEncryptionKey(initFactory)
            {
                Scope = PowerBIUserScope.Organization
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
        }
    }
}
