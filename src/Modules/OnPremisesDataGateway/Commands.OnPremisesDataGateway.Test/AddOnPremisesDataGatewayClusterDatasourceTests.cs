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
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class AddOnPremisesDataGatewayClusterDatasourceTests
    {
        private static CmdletInfo AddOnPremisesDataGatewayClusterDatasourceInfo { get; } = new CmdletInfo(
            $"{AddOnPremisesDataGatewayClusterDatasource.CmdletVerb}-{AddOnPremisesDataGatewayClusterDatasource.CmdletName}",
            typeof(AddOnPremisesDataGatewayClusterDatasource));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndAddOnPremisesDataGatewayClusterDatasource()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(AddOnPremisesDataGatewayClusterDatasourceInfo)
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasource.GatewayClusterId), Guid.NewGuid())
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasource.Annotation), "the annotation")
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasource.CredentialDetailsDictionary), new Dictionary<Guid, DatasourceCredentialDetails>
                    {
                        {   Guid.NewGuid(),
                            new DatasourceCredentialDetails
                            {
                                CredentialType = "Basic",
                                Credentials = "the enypted credentials",
                            }
                        }
                    })
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasource.DatasourceName), "the datasource name")
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasource.DatasourceType), "SQL")
                    .AddParameter(nameof(AddOnPremisesDataGatewayClusterDatasource.SingleSignOnType), "None");

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void AddOnPremisesDataGatewayClusterDatasourceReturnsExpectedResults()
        {
            // Arrange
            var gatewayClusterId = Guid.NewGuid();
            var gatewayClusterMemberId = Guid.NewGuid();

            var credentialDetailsDictionary = new Dictionary<Guid, DatasourceCredentialDetails>();
            credentialDetailsDictionary.Add(
                gatewayClusterId,
                new DatasourceCredentialDetails
                {
                    CredentialType = "Basic",
                    Credentials = "the encrypted credentials",
                    EncryptedConnection = "Encrypted",
                    EncryptionAlgorithm = "RSA-OAEP",
                    PrivacyLevel = "None",
                    SkipTestConnection = false,
                    UseCallerAADIdentity = false,
                    UseCustomOAuthApp = false,
                    UseEndUserOAuth2Credentials = false,
                });
            credentialDetailsDictionary.Add(
               gatewayClusterMemberId,
               new DatasourceCredentialDetails
               {
                   CredentialType = "Basic",
                   Credentials = "the encrypted credentials string",
                   EncryptedConnection = "Encrypted",
                   EncryptionAlgorithm = "RSA-OAEP",
                   PrivacyLevel = "None",
                   SkipTestConnection = false,
                   UseCallerAADIdentity = false,
                   UseCustomOAuthApp = false,
                   UseEndUserOAuth2Credentials = false,

               });

            var request = new PublishDatasourceToGatewayClusterRequest()
            {
                Annotation = "the annotation",
                ConnectionDetails = "the connection details",
                CredentialDetailsDictionary = credentialDetailsDictionary,
                DatasourceName = "the datasource name",
                DatasourceType = "SQL",
                SingleSignOnType = "None",
            };

            var expectedResponse = new GatewayClusterDatasource
            {
                ClusterId = gatewayClusterId,
                Id = Guid.NewGuid(),
                ConnectionDetails = request.ConnectionDetails,
                CredentialType = request.DatasourceType,
                DatasourceErrorDetails = new List<DatasourceErrorDetails>(),
                DatasourceName = request.DatasourceName,
                DatasourceType = request.DatasourceType,
                Users = new List<UserAccessRightEntry>(),
            };

            var client = new Mock<IPowerBIApiClient>();
            client.Setup(x => x.Gateways
                .CreateGatewayClusterDatasource(gatewayClusterId, It.IsAny<PublishDatasourceToGatewayClusterRequest>(), true))
                .ReturnsAsync(expectedResponse);

            var initFactory = new TestPowerBICmdletInitFactory(client.Object);
            var cmdlet = new AddOnPremisesDataGatewayClusterDatasource(initFactory)
            {
                GatewayClusterId = gatewayClusterId,
                Annotation = request.Annotation,
                ConnectionDetails = request.ConnectionDetails,
                CredentialDetailsDictionary = request.CredentialDetailsDictionary,
                DatasourceName = request.DatasourceName,
                DatasourceType = request.DatasourceType,
                SingleSignOnType = request.SingleSignOnType,
            };

            // Act
            cmdlet.InvokePowerBICmdlet();

            // Assert
            TestUtilities.AssertExpectedUnitTestResults(expectedResponse, client, initFactory);
        }
    }
}
