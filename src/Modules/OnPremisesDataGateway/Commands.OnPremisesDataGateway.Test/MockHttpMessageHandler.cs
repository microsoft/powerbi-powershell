/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerBI.Commands.OnPremisesDataGateway.Test
{
    public sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        public int SendAsyncInvocationCount
        {
            get;
            private set;
        }

        public Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> SendAsyncMockHandler { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken  cancellationToken)
        {
            this.SendAsyncInvocationCount++;
            return Task.FromResult(this.SendAsyncMockHandler != null ? 
                                       this.SendAsyncMockHandler(request, cancellationToken) 
                                       : request.CreateResponse(HttpStatusCode.OK));
        }

        public void Reset()
        {
            this.SendAsyncInvocationCount = 0;
            this.SendAsyncMockHandler     = null;
        }
    }
}
