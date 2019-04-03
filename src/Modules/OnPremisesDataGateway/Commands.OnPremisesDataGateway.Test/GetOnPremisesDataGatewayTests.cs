using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Commands.Common.Test;
using Microsoft.PowerBI.Commands.Profile.Test;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
        public async Task GetOnPremisesDataGatewayClusterTest()
        {
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
            ""appIds"": [],
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

            var result = await client.GetGatewayClusters(true);

        }
    }
}
