/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class SetOnPremisesDataGatewayClusterDatasourceCredentialsTests
    {
        private static CmdletInfo SetOnPremisesDataGatewayClusterDatasourceCredentialsInfo { get; } = new CmdletInfo(
            $"{SetOnPremisesDataGatewayClusterDatasourceCredentials.CmdletVerb}-{SetOnPremisesDataGatewayClusterDatasourceCredentials.CmdletName}",
            typeof(SetOnPremisesDataGatewayClusterDatasourceCredentials));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndSetOnPremisesDataGatewayClusterDatasourceCredentials()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(SetOnPremisesDataGatewayClusterDatasourceCredentialsInfo)
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.GatewayClusterDatasourceId), Guid.NewGuid())
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.Credentials), "the credentials string")
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.CredentialType), "Basic")
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.EncryptedConnection), "Encrypted")
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.EncryptionAlgorithm), "RSA-OAEP")
                    .AddParameter(nameof(SetOnPremisesDataGatewayClusterDatasourceCredentials.PrivacyLevel), "None");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void SetOnPremisesDataGatewayClusterDatasourceCredentialsReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var GatewayClusterDatasourceId = Guid.NewGuid();

            var request = new DatasourceCredentialDetails
            {
                Credentials = "the encrypted credentials string",
                CredentialType = "Basic",
                EncryptedConnection = "Encrypted",
                EncryptionAlgorithm = "RSA-OAEP",
                PrivacyLevel = "None",
                SkipTestConnection = false,
                UseCallerAADIdentity = false,
                UseCustomOAuthApp = false,
                UseEndUserOAuth2Credentials = false,
            };

            var expectedResponse = new DatasourceErrorDetails
            {
                GatewayName = "the gateway name 1",
                ErrorCode = "the error code 1",
                ErrorMessage = "the error message 1",
            };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .UpdateGatewayClusterDatasourceCredentials(gatewayClusterId, GatewayClusterDatasourceId, It.IsAny<DatasourceCredentialDetails>(), true))
                .ReturnsAsync(new[] { expectedResponse });

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new SetOnPremisesDataGatewayClusterDatasourceCredentials(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                GatewayClusterDatasourceId = GatewayClusterDatasourceId,
                Credentials = request.Credentials,
                CredentialType = request.CredentialType,
                EncryptedConnection = request.EncryptedConnection,
                EncryptionAlgorithm = request.EncryptionAlgorithm,
                PrivacyLevel = request.PrivacyLevel,
                SkipTestConnection = request.SkipTestConnection,
                UseCallerAADIdentity = request.UseCallerAADIdentity,
                UseCustomOAuthApp = request.UseCustomOAuthApp,
                UseEndUserOAuth2Credentials = request.UseEndUserOAuth2Credentials,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
