/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Client
{
    public class PowerBIHttpClientHandler : HttpClientHandler
    {
        private IPowerBILogger Logger { get; }

        public PowerBIHttpClientHandler(IPowerBILogger logger)
        {
            this.Logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Followed approach from (minus using DelegatingHandler as HttpClient needs HttpClientHandler) - https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/httpclient-message-handlers
            request.Headers.UserAgent.Clear(); // removes SDK header
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("MicrosoftPowerBIMgmt", PowerBIClientCmdlet.CmdletVersion));

            var response = await base.SendAsync(request, cancellationToken);
            this.Logger?.WriteVerbose($"Request Uri: {response.RequestMessage.RequestUri}");
            this.Logger?.WriteVerbose($"Status Code: {response.StatusCode} ({(int)response.StatusCode})");

            return response;
        }
    }
}
