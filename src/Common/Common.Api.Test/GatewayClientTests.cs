/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Common.Api.Test
{
    [TestClass]
    public class GatewayClientTests
    {
        [TestMethod]
        public async Task AddOnPremisesDataGatewayClusterUserCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new GatewayClusterAddPrincipalRequest()
            {
                PrincipalObjectId = Guid.NewGuid(),
                AllowedDataSourceTypes = new DatasourceType[]
                {
                    DatasourceType.Sql
                },
                Role = "the role"

            };

            // Act
            var result = await client.AddUsersToGatewayCluster(Guid.NewGuid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterStatusJsonResponseCanBeDeSerialized()
        {
            // Arrange
            var clusterStatus = "the cluster status";
            var serializedODataResponse = $@"{{
  ""@odata.context"":""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/status/$entity"",
  ""clusterStatus"":""{clusterStatus}"",
  ""gatewayStaticCapabilities"":""the static capabilities"",
  ""gatewayVersion"":""3000.0.0.0+gabcdef0"",
  ""gatewayUpgradeState"":""the upgrade state""
}}";

            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetGatewayClusterStatus(Guid.NewGuid(), true);

            // Assert
            result.ClusterStatus.Should().Be(clusterStatus);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterStatusCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterStatus = "the cluster status";
            var oDataResponse = new ODataResponseGatewayClusterStatusResponse
            {
                ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/status/$entity",
                ClusterStatus = clusterStatus,
                GatewayStaticCapabilities = "the static capabilities",
                GatewayVersion = "3000.0.0.0+gabcdef0",
                GatewayUpgradeState = "the upgrade state"
            };

            var serializedODataResponse = JsonConvert.SerializeObject(oDataResponse, new Newtonsoft.Json.Converters.StringEnumConverter());
            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetGatewayClusterStatus(Guid.NewGuid(), true);

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersJsonResponseCanBeDeSerialized()
        {
            // Arrange
            var clusterId = Guid.NewGuid();
            var serializedODataResponse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters"",
    ""value"": [
        {{
            ""id"": ""{clusterId}"",
            ""name"": ""cluster"",
            ""dataSourceIds"": [],
            ""permissions"": [
                {{
                    ""id"": ""{Guid.NewGuid()}"",
                    ""principalType"": ""User"",
                    ""tenantId"": ""{Guid.NewGuid()}"",
                    ""role"": ""Admin"",
                    ""allowedDataSources"": [],
                    ""clusterId"": ""{clusterId}""
                }}
            ],
            ""memberGateways"": [
                {{
                    ""id"": ""{clusterId}"",
                    ""name"": ""gateway1"",
                    ""type"": ""Resource"",
                    ""version"": ""3000.0.0"",
                    ""clusterId"": ""{clusterId}""
                }}
            ]
        }}
    ]
}}";
            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetGatewayClusters(true);
            var resultCluster = result.ToArray()[0];

            // Assert
            // Assumption: we leave it to other tests to make sure all values are correct
            // The main purpose of this test is to make sure de-serialization works
            resultCluster.Id.Should().Be(clusterId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterId = Guid.NewGuid();
            var gatewayClusterObject = new GatewayCluster
            {
                Id = clusterId,
                Name = "the cluster name",
                Description = "the cluster description",
                Status = "the status",
                Region = "the region",
                Permissions = new Permission[]
                {
                    new Permission
                    {
                        Id = Guid.NewGuid().ToString(),
                        PrincipalType = "User",
                        TenantId = Guid.NewGuid().ToString(),
                        Role = "Admin",
                        AllowedDataSources = new string[]
                        {
                            Guid.NewGuid().ToString()
                        },
                        ClusterId = clusterId.ToString()
                    }
                },
                DataSourceIds = new Guid[]
                {
                    Guid.NewGuid()
                },
                Type = "the type",
                MemberGateways = new MemberGateway[]
                {
                    new MemberGateway
                    {
                        Id = clusterId,
                        Name = "the member name",
                        Description = "the member description",
                        Status = "the member status",
                        Region = "the member region",
                        Type = "the member type",
                        Version = "the version",
                        Annotation = "the annotation",
                        ClusterId = clusterId
                    }
                }
            };

            var oDataResponse = new ODataResponseList<GatewayCluster>
            {
                ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusters",
                Value = new GatewayCluster[]
                {
                    gatewayClusterObject
                }
            };

            var serializedODataResponse = JsonConvert.SerializeObject(oDataResponse, new Newtonsoft.Json.Converters.StringEnumConverter());
            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetGatewayClusters(true);

            // Assert
            gatewayClusterObject.Should().BeEquivalentTo(result.ToArray()[0]);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersWithClusterIdJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var clusterId = Guid.NewGuid();
            var serializedOdataResponse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/$entity"",
    ""id"": ""{clusterId}"",
    ""name"": ""cluster"",
    ""dataSourceIds"": [],
    ""permissions"": [
        {{
            ""id"": ""{Guid.NewGuid()}"",
            ""principalType"": ""User"",
            ""tenantId"": ""{Guid.NewGuid()}"",
            ""role"": ""Admin"",
            ""allowedDataSources"": [],
            ""clusterId"": ""{clusterId}""
        }}
    ],
    ""memberGateways"": [
        {{
            ""id"": ""{clusterId}"",
            ""name"": ""gateway1"",
            ""type"": ""Resource"",
            ""version"": ""3000.0.0"",
            ""clusterId"": ""{clusterId}""
        }}
    ]
}}";
            var client = Utilities.GetTestClient(serializedOdataResponse);

            // Act
            var result = await client.GetGatewayClusters(clusterId, true);

            // Assert
            result.Id.Should().Be(clusterId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersWithClusterIdCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterId = Guid.NewGuid();
            var oDataResponse = new ODataResponseGatewayCluster
            {
                ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/$entity",
                Id = clusterId,
                Name = "the cluster name",
                Description = "the cluster description",
                Status = "the status",
                Region = "the region",
                Permissions = new Permission[]
                {
                    new Permission
                    {
                        Id = Guid.NewGuid().ToString(),
                        PrincipalType = "User",
                        TenantId = Guid.NewGuid().ToString(),
                        Role = "Admin",
                        AllowedDataSources = new string[]
                        {
                            Guid.NewGuid().ToString()
                        },
                        ClusterId = clusterId.ToString()
                    }
                },
                DataSourceIds = new Guid[]
                {
                    Guid.NewGuid()
                },
                Type = "the type",
                MemberGateways = new MemberGateway[]
                {
                    new MemberGateway
                    {
                        Id = clusterId,
                        Name = "the member name",
                        Description = "the member description",
                        Status = "the member status",
                        Region = "the member region",
                        Type = "the member type",
                        Version = "the version",
                        Annotation = "the annotation",
                        ClusterId = clusterId
                    }
                }
            };

            var serializedODataResponse = JsonConvert.SerializeObject(oDataResponse, new Newtonsoft.Json.Converters.StringEnumConverter());
            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetGatewayClusters(clusterId, true);

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayTenantPolicyJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var tenantId = Guid.NewGuid().ToString();
            var serializedODataResponse = $@"{{
  ""@odata.context"":""http://example.net/v2.0/myorg/gatewayPolicy"",
  ""id"":""{tenantId}"",
  ""policy"":""None""
}}";

            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetTenantPolicy();

            // Assert
            result.TenantObjectId.Should().Be(tenantId);
            result.Policy.Should().Be(TenantPolicy.None);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayTenantPolicyCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();
            var oDataResponse = new ODataResponseGatewayTenant
            {
                ODataContext = "http://example.net/v2.0/myorg/gatewayPolicy",
                TenantObjectId = tenantId,
                Policy = TenantPolicy.None
            };

            var serializedODataResponse = JsonConvert.SerializeObject(oDataResponse, new Newtonsoft.Json.Converters.StringEnumConverter());
            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetTenantPolicy();

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayInstallersJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var principalObjectId = Guid.NewGuid().ToString();
            var serializedODataResponse = $@"{{
  ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayInstallers"",
  ""value"": [
    {{
      ""id"":""{principalObjectId}"",
      ""type"":""Personal""
    }}
  ]
}}";

            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetInstallerPrincipals(GatewayType.Personal);

            // Assert
            result.ToArray()[0].PrincipalObjectId.Should().Be(principalObjectId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayInstallersCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var principalObjectId = Guid.NewGuid().ToString();
            var oDataResponse = new ODataResponseList<InstallerPrincipal>
                                {
                                    ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayInstallers",
                                    Value = new[]
                                            {
                                                new InstallerPrincipal
                                                {
                                                    PrincipalObjectId = principalObjectId,
                                                    GatewayType       = GatewayType.Personal.ToString()
                                                }
                                            }
                                };

            var serializedODataResponse = JsonConvert.SerializeObject(
                oDataResponse,
                new Newtonsoft.Json.Converters.StringEnumConverter());
            var client = Utilities.GetTestClient(serializedODataResponse);

            // Act
            var result = await client.GetInstallerPrincipals(GatewayType.Personal);

            // Assert
            oDataResponse.Value.Should().BeEquivalentTo(result);
        }

        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterMemberCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.DeleteGatewayClusterMember(Guid.NewGuid(), Guid.NewGuid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.DeleteGatewayCluster(Guid.NewGuid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterUserCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.DeleteUserOnGatewayCluster(Guid.NewGuid(), Guid.NewGuid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task SetOnPremisesDataGatewayClusterCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new PatchGatewayClusterRequest()
            {
                Name = "name",
                Department = "department",
                Description = "description",
                ContactInformation = "contactInformation",
                AllowCloudDatasourceRefresh = true,
                AllowCustomConnectors = true,
                LoadBalancingSelectorType = "loadBalancingSelectorType"
            };

            // Act
            var result = await client.PatchGatewayCluster(Guid.NewGuid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task SetOnPremisesDataGatewayInstallersCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayInstallersRequest
            {
                Ids = new string[]
                {
                    "id"
                },
                Operation = OperationType.None,
                GatewayType = GatewayType.Personal
            };

            // Act
            var result = await client.UpdateInstallerPrincipals(request);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task SetOnPremisesDataGatewayTenantPolicyCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayPolicyRequest()
            {
                ResourceGatewayInstallPolicy = PolicyType.Restricted
            };

            // Act
            var result = await client.UpdateTenantPolicy(request);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterDatasourcesCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterId = Guid.NewGuid();
            var datasourceId = Guid.NewGuid();
            var serializedODataRepsonse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayClusterDatasources"",
    ""value"": [
        {{
            ""id"": ""{datasourceId}"",
            ""clusterId"": ""{clusterId}"",
            ""datasourceType"": ""AnalysisServices"",
            ""connectionDetails"": ""\'server\':\'egwsql\\\\sql2012tm\',\'database\':\'advworks\'"",
            ""credentialType"": ""Windows"",
            ""datasourceName"": ""testserver\testdatasource"",
            ""datasourceErrorDetails"": []
        }}
    ]
}}";
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetGatewayClusterDatasources(clusterId, true);

            // Assert
            var resultDatasource = result.ToArray()[0];

            // Assert
            resultDatasource.ClusterId.Should().Be(clusterId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterDatasourceCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterId = Guid.NewGuid();
            var datasourceId = Guid.NewGuid();
            var oDataResponse = new ODataResponseGatewayClusterDatasource
            {
                ODataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusterDatasource",
                ClusterId = clusterId,
                Id = datasourceId,
                DatasourceType = "AnalysisServices",
                ConnectionDetails = "{\"server\":\"testserver\\\\testdatabase\",\"database\":\"testdatasource\"}",
                CredentialType = "Windows",
                DatasourceName = @"testserver\testdatabase",
                DatasourceErrorDetails = new List<DatasourceErrorDetails>(),
            };

            var serializedODataRepsonse = JsonConvert.SerializeObject(oDataResponse, new Newtonsoft.Json.Converters.StringEnumConverter());
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetGatewayClusterDatasource(clusterId, datasourceId, true);

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterDatasourceStatusCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.GetGatewayClusterDatasourceStatus(Guid.NewGuid(), Guid.NewGuid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task AddOnPremisesDataGatewayClusterDatasourceCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new DatasourceUser()
            {
                DatasourceUserAccessRight = "Read",
                DisplayName = "the user displayName",
                Identifier = Guid.NewGuid().ToString(),
                PrincipalType = "User",
                EmailAddress = "theEmailAddress@foo.com",
            };

            // Act
            var result = await client.AddUsersToGatewayClusterDatasource(Guid.NewGuid(), Guid.NewGuid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task SetOnPremisesDataGatewayClusterDatasourceCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new UpdateGatewayClusterDatasourceRequest
            {
                Annotation = "the annotation",
                DatasourceName = "the datasource name",
                SingleSignOnType = "None",
            };

            // Act
            var result = await client.UpdateGatewayClusterDatasource(Guid.NewGuid(), Guid.NewGuid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterDatasourceCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.DeleteGatewayClusterDatasource(Guid.NewGuid(), Guid.NewGuid(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task AddOnPremisesDataGatewayClusterDatasourceUserCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");
            var request = new DatasourceUser()
            {
                DatasourceUserAccessRight = "Read",
                DisplayName = "the user displayName",
                Identifier = Guid.NewGuid().ToString(),
                PrincipalType = "User",
                EmailAddress = "theEmailAddress@foo.com",
            };

            // Act
            var result = await client.AddUsersToGatewayClusterDatasource(Guid.NewGuid(), Guid.NewGuid(), request, true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterDatasourceUsersCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var userEmailAddress = "theemailAddress@foo.com";
            var serializedODataRepsonse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#users"",
    ""value"": [
        {{
            ""emailAddress"":""{userEmailAddress}"",
            ""datasourceAccessRight"":""Read"",
            ""displayName"":""the email address"",
            ""identifier"":""{userEmailAddress}"",
            ""principalType"":""User""
        }}
    ]
}}";
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetGatewayClusterDatasourceUsers(Guid.NewGuid(), Guid.NewGuid(), true);

            // Assert
            var resultUsers = result.ToArray()[0];

            // Assert
            resultUsers.EmailAddress.Should().Be(userEmailAddress);
        }

        [TestMethod]
        public async Task RemoveOnPremisesDataGatewayClusterDatasourceUserCanBeSerialized()
        {
            // Arrange
            var client = Utilities.GetTestClient("");

            // Act
            var result = await client.RemoveGatewayClusterDatasourceUser(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid().ToString(), true);

            // Assert
            result.IsSuccessStatusCode.Should().Be(true);
        }

        [TestMethod]
        public async Task SetOnPremisesDataGatewayClusterDatasourceCredentialsCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var errorCode = Guid.NewGuid().ToString();
            var serializedODataRepsonse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#credentials"",
    ""value"": [
        {{          
            ""ErrorCode"":""{errorCode}"",
            ""ErrorMessage"":""the error message"",
            ""GatewayName"":""the gateway name"",
        }}
    ]
}}";
            var client = Utilities.GetTestClient(serializedODataRepsonse);
            var request = new DatasourceCredentialDetails
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
            };

            // Act
            var result = await client.UpdateGatewayClusterDatasourceCredentials(Guid.NewGuid(), Guid.NewGuid(), request, true);

            // Assert
            var resultErrors = result.ToArray()[0];
            resultErrors.ErrorCode.Should().Be(errorCode);
        }
    }
}
