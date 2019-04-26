/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerBI.Common.Abstractions.Utilities
{
    public static class HttpClientExtensions
    {
        // TODO: DELETE THIS once we target > .NET (standard|core) 2.0 
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            var response = await client.SendAsync(request, CancellationToken.None);
            return response;
        }
    }
}
