/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Net.Http;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Common.Api
{
    public class PowerBIApiClient : IPowerBIApiClient
    {
        private bool disposed;

        private IPowerBIClient Client { get; set; }

        public IReportsClient Reports { get; set; }

        public IWorkspacesClient Workspaces { get; set; }

        public IDatasetsClient Datasets { get; set; }

        public PowerBIApiClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            this.Client = CreateClient(authenticator, profile, logger, settings);
            InitializeClients();
        }

        public PowerBIApiClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, HttpClientHandler httpClientHandler)
        {
            this.Client = CreateClient(authenticator, profile, logger, settings, httpClientHandler);
            InitializeClients();
        }

        private void InitializeClients()
        {
            this.Reports = new ReportsClient(this.Client);
            this.Workspaces = new WorkspacesClient(this.Client);
            this.Datasets = new DatasetsClient(this.Client);
        }

        private static IPowerBIClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            PowerBIClient client = null;
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                client = new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken));
            }
            else
            {
                client = new PowerBIClient(new TokenCredentials(token.AccessToken));
            }

            SetTimeoutForClient(client, logger, settings);
            return client;
        }

        private static IPowerBIClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, HttpClientHandler httpClientHandler)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            PowerBIClient client = null;
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                client = new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken), httpClientHandler);
            }
            else
            {
                client = new PowerBIClient(new TokenCredentials(token.AccessToken), httpClientHandler);
            }

            SetTimeoutForClient(client, logger, settings);
            return client;
        }

        private static void SetTimeoutForClient(PowerBIClient client, IPowerBILogger logger, IPowerBISettings settings)
        {
            // Set HttpTimeout if not null to HttpClient that PowerBIClient contains
            if (settings.Settings.HttpTimeout.HasValue)
            {
                logger.WriteVerbose($"Setting HTTP client timeout to {settings.Settings.HttpTimeout.Value}");
                client.HttpClient.Timeout = settings.Settings.HttpTimeout.Value;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PowerBIApiClient()
        {
            // Finalizer calls Dispose(false)  
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            if (disposing)
            {
                // Free managed resources  
                if (this.Client != null)
                {
                    this.Client.Dispose();
                    this.Client = null;
                }

                if (this.Reports != null)
                {
                    this.Reports = null;
                }

                if (this.Workspaces != null)
                {
                    this.Workspaces = null;
                }

                if(this.Datasets != null)
                {
                    this.Datasets = null;
                }
            }
        }
    }
}
