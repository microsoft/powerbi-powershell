/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayClustersTests
    {
        private static CmdletInfo GetOnPremisesDataGatewayClustersInfo { get; } = new CmdletInfo(
            $"{GetOnPremisesDataGatewayClusters.CmdletVerb}-{GetOnPremisesDataGatewayClusters.CmdletName}",
            typeof(GetOnPremisesDataGatewayClusters));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayClusters()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayClustersInfo);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayClustersWithGatewayClusterId()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayClustersInfo).AddParameter(nameof(GetOnPremisesDataGatewayClusters.GatewayClusterId), new Guid("{1C4781C9-1767-4D4B-919E-7DA2BDD81AF4}"));

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersJsonResponseCanBeDeSerialized()
        {
            // Arrange
            var clusterId = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C");
            var serializedODataRepsonse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters"",
    ""value"": [
        {{
            ""id"": ""{clusterId}"",
            ""name"": ""cluster"",
            ""dataSourceIds"": [],
            ""permissions"": [
                {{
                    ""id"": ""490F8A04-ABB8-4015-ABE6-7D361B9135B3"",
                    ""principalType"": ""User"",
                    ""tenantId"": ""7552F012-6C5E-4E5B-8EA2-260215EB8236"",
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
            var client = Utilities.GetTestClient(serializedODataRepsonse);

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
            var clusterId = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C");
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
                        Id = "490F8A04-ABB8-4015-ABE6-7D361B9135B3",
                        PrincipalType = "User",
                        TenantId = "7552F012-6C5E-4E5B-8EA2-260215EB8236",
                        Role = "Admin",
                        AllowedDataSources = new string[]
                        {
                            "EFD4D249-519D-4CE5-BC2D-F7607C30EC02"
                        },
                        ClusterId = clusterId.ToString()
                    }
                },
                DataSourceIds = new Guid[]
                {
                    new Guid("EFD4D249-519D-4CE5-BC2D-F7607C30EC02")
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

            var serializedODataRepsonse = JsonConvert.SerializeObject(oDataResponse);
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetGatewayClusters(true);

            // Assert
            gatewayClusterObject.Should().BeEquivalentTo(result.ToArray()[0]);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersWithClusterIdJsonResponseCanBeDeSerialized()
        {

            // Arrange
            var clusterId = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C");
            var serializedOdataRepsonse = $@"{{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters/$entity"",
    ""id"": ""{clusterId}"",
    ""name"": ""cluster"",
    ""dataSourceIds"": [],
    ""permissions"": [
        {{
            ""id"": ""490F8A04-ABB8-4015-ABE6-7D361B9135B3"",
            ""principalType"": ""User"",
            ""tenantId"": ""7552F012-6C5E-4E5B-8EA2-260215EB8236"",
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
            var client = Utilities.GetTestClient(serializedOdataRepsonse);

            // Act
            var result = await client.GetGatewayClusters(clusterId, true);

            // Assert
            result.Id.Should().Be(clusterId);
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClustersWithClusterIdCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var clusterId = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C");
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
                        Id = "490F8A04-ABB8-4015-ABE6-7D361B9135B3",
                        PrincipalType = "User",
                        TenantId = "7552F012-6C5E-4E5B-8EA2-260215EB8236",
                        Role = "Admin",
                        AllowedDataSources = new string[]
                        {
                            "EFD4D249-519D-4CE5-BC2D-F7607C30EC02"
                        },
                        ClusterId = clusterId.ToString()
                    }
                },
                DataSourceIds = new Guid[]
                {
                    new Guid("EFD4D249-519D-4CE5-BC2D-F7607C30EC02")
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

            var serializedODataRepsonse = JsonConvert.SerializeObject(oDataResponse);
            var client = Utilities.GetTestClient(serializedODataRepsonse);

            // Act
            var result = await client.GetGatewayClusters(clusterId, true);

            // Assert
            oDataResponse.Should().BeEquivalentTo(result);
        }
    }
}
