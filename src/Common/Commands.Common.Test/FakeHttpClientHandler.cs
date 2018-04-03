/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class FakeHttpClientHandler : HttpClientHandler
    {
        public HttpRequestMessage Request { get; private set; }

        private HttpResponseMessage ResponseMessage { get; }

        public FakeHttpClientHandler(HttpResponseMessage responseMessage) => this.ResponseMessage = responseMessage;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.Request = request;
            return Task.FromResult(this.ResponseMessage);
        }
    }
}
