/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Common.Client
{
    public abstract class PowerBIClientCmdlet : PowerBICmdlet
    {
        public PowerBIClientCmdlet() : base()
        {
        }

        public PowerBIClientCmdlet(IPowerBICmdletInitFactory init) : base(init)
        {
        }

        protected virtual IPowerBIClient CreateClient()
        {
            var token = this.Authenticator.Authenticate(this.Profile, this.Logger, this.Settings);
            if (Uri.TryCreate(this.Profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                return new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken));
            }
            else
            {
                return new PowerBIClient(new TokenCredentials(token.AccessToken));
            }
        }
    }
}
