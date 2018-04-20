/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestClient : IPowerBIClientFactory
    {
        public IPowerBIApiClient Client { get; }

        public FakeHttpClientHandler ClientHandler { get; }

        public TestClient(IPowerBIApiClient client)
        {
            this.Client = client;
        }

        public TestClient(FakeHttpClientHandler handler)
        {
            this.ClientHandler = handler;
        }

        public IPowerBIApiClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            return this.Client ?? new PowerBIApiClient(authenticator, profile, logger, settings, this.ClientHandler);
        }
    }
}
