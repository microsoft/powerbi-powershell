/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Capacities;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.Capacities.Test
{
    [TestClass]
    public class GetPowerBICapacityTests
    {
        private static CmdletInfo GetPowerBICapacityCmdletInfo => new CmdletInfo($"{GetPowerBICapacity.CmdletVerb}-{GetPowerBICapacity.CmdletName}", typeof(GetPowerBICapacity));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBICapacityIndividualScopeCmdletInfo()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBICapacityCmdletInfo);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No capacities returned. Verify you have capacity in your organization.");
                }
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetPowerBICapacityOrganizationScopeCmdletInfo()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetPowerBICapacityCmdletInfo)
                    .AddParameter(nameof(GetPowerBICapacity.Scope), PowerBIUserScope.Organization);

                // Act
                var results = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(results);
                if (!results.Any())
                {
                    Assert.Inconclusive("No capacities returned. Verify you have capacity in your organization.");
                }
            }
        }

        [TestMethod]
        public void GetPowerBICapacity_IndividualScope()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var capacities = SetupPowerBICapacityMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBICapacity(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Capacities.GetCapacities(), Times.Once());
            client.Verify(x => x.Capacities.GetCapacitiesAsAdmin(null), Times.Never);
            AssertExpectedUnitTestResults(capacities.Item1, initFactory);
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
        }

        [TestMethod]
        public void GetPowerBICapacity_OrganizationScope()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var capacities = SetupPowerBICapacityMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBICapacity(initFactory)
            {
                Scope = PowerBIUserScope.Organization
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Capacities.GetCapacities(), Times.Never);
            client.Verify(x => x.Capacities.GetCapacitiesAsAdmin(null), Times.Once());
            AssertExpectedUnitTestResults(capacities.Item2, initFactory);
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
        }

        [TestMethod]
        public void GetPowerBICapacity_OrganizationScope_WithShowOnEncryption()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var capacities = SetupPowerBICapacityMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBICapacity(initFactory)
            {
                Scope = PowerBIUserScope.Organization,
                Show = PowerBIGetCapacityExpandEnum.EncryptionKey
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            client.Verify(x => x.Capacities.GetCapacities(), Times.Never);
            client.Verify(x => x.Capacities.GetCapacitiesAsAdmin("tenantKey"), Times.Once());
            AssertExpectedUnitTestResults(capacities.Item2, initFactory);
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
        }

        [TestMethod]
        public void GetPowerBICapacity_IndividualScope_WithShowOnEncryption()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            var capacities = SetupPowerBICapacityMock(client);
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBICapacity(initFactory)
            {
                Show = PowerBIGetCapacityExpandEnum.EncryptionKey
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Show on EncryptionKey is only applied when -Scope is set to Organization");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetPowerBICapacity_WithGetCapacitiesApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Capacities.GetCapacities()).Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBICapacity(initFactory);

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Some exception");
            client.Verify(x => x.Capacities.GetCapacities(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetPowerBICapacity_WithGetCapacitiesAsAdminApiThrowingException()
        {
            // Arrange
            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Capacities.GetCapacitiesAsAdmin(It.IsAny<string>()))
                .Throws(new Exception("Some exception"));
            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new GetPowerBICapacity(initFactory)
            {
                Scope = PowerBIUserScope.Organization
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            var throwingErrorRecords = initFactory.Logger.ThrowingErrorRecords;
            Assert.AreEqual(throwingErrorRecords.Count(), 1);
            Assert.AreEqual(throwingErrorRecords.First().ToString(), "Some exception");
            client.Verify(x => x.Capacities.GetCapacitiesAsAdmin(null), Times.Once());
        }

        private static void AssertExpectedUnitTestResults(IEnumerable<Capacity> capacities, TestPowerBICmdletInitFactory initFactory)
        {
            Assert.IsFalse(initFactory.Logger.ErrorRecords.Any());
            var results = initFactory.Logger.Output.ToList();
            Assert.AreEqual(results.Count, capacities.Count());
            CollectionAssert.AreEqual(capacities.ToList(), results.ToList());
        }

        private Tuple<List<Capacity>, List<Capacity>> SetupPowerBICapacityMock(Mock<IPowerBIApiClient> client)
        {
            var capacity1 = new Capacity()
            {
                Id = Guid.NewGuid(),
                DisplayName = "Capacity1",
                Admins = new List<string> { "Admin1" },
                Sku = "P1",
                State = Capacity.CapacityState.Active,
                UserAccessRight = Capacity.CapacityUserAccessRight.None,
                Region = "West Central US",
                EncryptionKeyId = null
            };

            var encryptionKeyId = Guid.NewGuid();
            var capacity2 = new Capacity()
            {
                Id = Guid.NewGuid(),
                DisplayName = "Capacity2",
                Admins = new List<string> { "Admin1", "Admin2" },
                Sku = "A1",
                State = Capacity.CapacityState.Invalid,
                UserAccessRight = Capacity.CapacityUserAccessRight.Admin,
                Region = "West Central US",
                EncryptionKeyId = encryptionKeyId,
                EncryptionKey = new EncryptionKey()
                {
                    Id = encryptionKeyId,
                    Name = "KeyName",
                    KeyVaultKeyIdentifier = new Uri("http://www.contoso.com/"),
                    IsDefault = false,
                    CreatedAt = new DateTime(1995, 1, 1),
                    UpdatedAt = new DateTime(1995, 1, 1)
                }
            };

            var getCapacitiesResponse = new List<Capacity>() { capacity1 };
            var getCapacitiesAsAdminResponse = new List<Capacity>() { capacity1, capacity2 };

            client.Setup(x => x.Capacities.GetCapacities()).Returns(getCapacitiesResponse);
            client.Setup(x => x.Capacities.GetCapacitiesAsAdmin(It.IsAny<string>())).Returns(getCapacitiesAsAdminResponse);

            return Tuple.Create(getCapacitiesResponse, getCapacitiesAsAdminResponse);
        }
    }
}
