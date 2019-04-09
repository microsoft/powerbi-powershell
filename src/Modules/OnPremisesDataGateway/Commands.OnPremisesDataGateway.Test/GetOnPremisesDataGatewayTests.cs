using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    [TestClass]
    public class GetOnPremisesDataGatewayTests
    {
        public static CmdletInfo GetOnPremisesDataGatewayCluster { get; } = new CmdletInfo(
            $"{OnPremisesDataGateway.GetOnPremisesDataGatewayCluster.CmdletVerb}-{OnPremisesDataGateway.GetOnPremisesDataGatewayCluster.CmdletName}",
            typeof(GetOnPremisesDataGatewayCluster));

        [TestMethod]
        [TestCategory("Interactive")]
        [TestCategory("SkipWhenLiveUnitTesting")] // Ignore for Live Unit Testing
        public void EndToEndGetOnPremisesDataGatewayCluster()
        {
            using (var ps = System.Management.Automation.PowerShell.Create())
            {
                // Arrange
                ProfileTestUtilities.ConnectToPowerBI(ps);
                ps.AddCommand(GetOnPremisesDataGatewayCluster);

                // Act
                var result = ps.Invoke();

                // Assert
                TestUtilities.AssertNoCmdletErrors(ps);
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterJsonResponseCanBeDeSerialized()
        {
            // Arrange
            var client = new GatewayV2Client(
                new Uri("https://bing.com"),
                new Mock<IAccessToken>().Object,
                new MockHttpMessageHandler
                {
                    SendAsyncMockHandler = (httpMessageRequest, cancellationToken) =>
                                           {
                                               var content = @"{
    ""@odata.context"": ""http://example.net/v2.0/myorg/me/$metadata#gatewayClusters"",
    ""value"": [
        {
            ""id"": ""B3497368-4FC0-409F-A338-827B3B7A6F1C"",
            ""name"": ""cluster"",
            ""dataSourceIds"": [],
            ""permissions"": [
                {
                    ""id"": ""490F8A04-ABB8-4015-ABE6-7D361B9135B3"",
                    ""principalType"": ""User"",
                    ""tenantId"": ""7552F012-6C5E-4E5B-8EA2-260215EB8236"",
                    ""role"": ""Admin"",
                    ""allowedDataSources"": [],
                    ""clusterId"": ""B3497368-4FC0-409F-A338-827B3B7A6F1C""
                }
            ],
            ""memberGateways"": [
                {
                    ""id"": ""B3497368-4FC0-409F-A338-827B3B7A6F1C"",
                    ""name"": ""gateway1"",
                    ""type"": ""Resource"",
                    ""version"": ""3000.0.0"",
                    ""clusterId"": ""B3497368-4FC0-409F-A338-827B3B7A6F1C""
                }
            ]
        }
    ]
}";
                                               var response = httpMessageRequest.CreateResponse(HttpStatusCode.OK);
                                               response.Content = new StringContent(
                                                   content,
                                                   Encoding.UTF8,
                                                   "application/json");
                                               return response;
                                           }
                });

            // Act
            var result = await client.GetGatewayClusters(true);

            // Assert -- we got past the above line :-) 
        }

        [TestMethod]
        public async Task GetOnPremisesDataGatewayClusterCanBeSerializedAndDeSerialized()
        {
            // Arrange
            var gatewayClusterObject = new GatewayCluster
            {
                Id = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C"),
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
                        ClusterId = "B3497368-4FC0-409F-A338-827B3B7A6F1C"
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
                        Id = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C"),
                        Name = "the member name",
                        Description = "the member description",
                        Status = "the member status",
                        Region = "the member region",
                        Type = "the member type",
                        Version = "the version",
                        Annotation = "the annotation",
                        ClusterId = new Guid("B3497368-4FC0-409F-A338-827B3B7A6F1C")
                    }
                }
            };

            var odataResponse = new ODataGatewayResponseList<GatewayCluster>{
                OdataContext = "http://example.net/v2.0/myorg/me/$metadata#gatewayClusters",
                Value = new GatewayCluster[]
                {
                    gatewayClusterObject
                }
            };

            var serializedOdataRepsonse = JsonConvert.SerializeObject(odataResponse);

            var client = new GatewayV2Client(
                new Uri("https://bing.com"),
                new Mock<IAccessToken>().Object,
                new MockHttpMessageHandler
                {
                    SendAsyncMockHandler = (httpMessageRequest, cancellationToken) =>
                    {
                        var content = serializedOdataRepsonse;
                        var response = httpMessageRequest.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(
                            content,
                            Encoding.UTF8,
                            "application/json");
                        return response;
                    }
                });

            // Act
            var result = await client.GetGatewayClusters(true);

            // Assert
            gatewayClusterObject.Should().BeEquivalentTo(result.ToArray()[0]);
        }

    }
}
