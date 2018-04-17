/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Client;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestClient : IPowerBIClientFactory
    {
        public FakeHttpClientHandler ClientHandler { get; }

        public TestClient(FakeHttpClientHandler handler)
        {
            this.ClientHandler = handler;
        }

        public IPowerBIClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                return new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken), this.ClientHandler);
            }
            else
            {
                return new PowerBIClient(new TokenCredentials(token.AccessToken), this.ClientHandler);
            }
        }
    }
}
