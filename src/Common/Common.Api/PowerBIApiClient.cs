﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Net.Http;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Common.Api
{
    public class PowerBIApiClient : IPowerBIApiClient, IDisposable
    {
        private readonly IPowerBIClient Client;

        public IReportsClient Reports { get; set; }

        public IWorkspacesClient Workspaces { get; set; }

        public PowerBIApiClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            this.Client = CreateClient(authenticator, profile, logger, settings);
            this.Reports = new ReportsClient(this.Client);
            this.Workspaces = new WorkspacesClient(this.Client);
        }

        public PowerBIApiClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, HttpClientHandler httpClientHandler)
        {
            this.Client = CreateClient(authenticator, profile, logger, settings, httpClientHandler);
            this.Reports = new ReportsClient(this.Client);
            this.Workspaces = new WorkspacesClient(this.Client);
        }

        private static IPowerBIClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                return new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken));
            }
            else
            {
                return new PowerBIClient(new TokenCredentials(token.AccessToken));
            }
        }

        private static IPowerBIClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, HttpClientHandler httpClientHandler)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                return new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken), httpClientHandler);
            }
            else
            {
                return new PowerBIClient(new TokenCredentials(token.AccessToken), httpClientHandler);
            }
        }

        public void Dispose()
        {
            this.Client.Dispose();
        }
    }
}
