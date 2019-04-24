/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Gateways;
using Moq;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    internal class Utilities
    {
        internal static GatewayV2Client GetTestClient(string serializedOdataRepsonse)
        {
            return new GatewayV2Client(
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
        }
    }
}
