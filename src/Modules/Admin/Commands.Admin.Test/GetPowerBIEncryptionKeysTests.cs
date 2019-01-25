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
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Admin.Test
{
    [TestClass]
    public class GetPowerBIEncryptionKeysTests
    {
        private static CmdletInfo GetPowerBIEncryptionKeysCmdletInfo => new CmdletInfo($"{GetPowerBIEncryptionKeys.CmdletVerb}-{GetPowerBIEncryptionKeys.CmdletName}", typeof(GetPowerBIEncryptionKeys));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBIEncryptionKeys()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps, PowerBIEnvironmentType.Public);
                ps.AddCommand(GetPowerBIEncryptionKeysCmdletInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
            }
        }

        [TestMethod]
        public void GetPowerBIEncryptionKeys_WithValidResponse()
        {
            // Arrange
            var encryptionKey1 = new EncryptionKey(Guid.NewGuid(), "KeyName1", "KeyVaultUri1", true, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            var encryptionKey2 = new EncryptionKey(Guid.NewGuid(), "KeyName2", "KeyVaultUri2", true, new DateTime(1995, 1, 1), new DateTime(1995, 1, 1));
            var encryptionKeys = new List<EncryptionKey>();
            encryptionKeys.Add(encryptionKey1);
            encryptionKeys.Add(encryptionKey2);
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Returns(encryptionKeys);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIEncryptionKeys(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Admin.GetPowerBIEncryptionKeys(), Times.Once());
        }

        [TestMethod]
        public void GetPowerBIEncryptionKeys_WithApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Admin.GetPowerBIEncryptionKeys()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBIEncryptionKeys(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.IsTrue(throwingErrorRecords.Count() > 0, "Should throw Exception");
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Some exception");
        }
    }
}
