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

namespace Microsoft.PowerBI.Common.Api.Test
{
    internal class Utilities
    {
        internal static GatewayClient GetTestClient(string serializedOdataRepsonse)
        {
            return new GatewayClient(
                new Uri("https://bing.com"),
                new Mock<IAccessToken>().Object,
                new MockHttpMessageHandler
                {
                    SendAsyncMockHandler = (httpMessageRequest, cancellationToken) =>
                    {
                        var content = serializedOdataRepsonse;
                        var response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(
                            content,
                            Encoding.UTF8,
                            "application/json")
                        };
                        return response;
                    }
                });
        }
    }
}
