/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Net.Http;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Gateways;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;
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

        public IGatewayClient Gateways { get; set; }

        public PowerBIApiClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            this.Client = CreateClient(authenticator, profile, logger, settings);
            this.Gateways = CreateGatewaysClient(authenticator, profile, logger, settings);
            InitializeClients();
        }

        public PowerBIApiClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, HttpClientHandler httpClientHandler)
        {
            this.Client = CreateClient(authenticator, profile, logger, settings, httpClientHandler);
            this.Gateways = CreateGatewaysV2Client(authenticator, profile, logger, settings, httpClientHandler);
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

        private static IGatewayClient CreateGatewaysClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                return new GatewayClient(baseUri, token);
            }

            throw new ArgumentNullException(nameof(IPowerBIEnvironment.GlobalServiceEndpoint));
        }

        private static IGatewayClient CreateGatewaysV2Client(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, HttpClientHandler httpClientHandler)
        {
            var token = authenticator.Authenticate(profile, logger, settings);
            if (Uri.TryCreate(profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                return new GatewayClient(baseUri, token, httpClientHandler);
            }

            throw new ArgumentNullException(nameof(IPowerBIEnvironment.GlobalServiceEndpoint));
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

                if(this.Gateways != null)
                {
                    this.Gateways = null;
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
