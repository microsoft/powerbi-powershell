/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api;

namespace Microsoft.PowerBI.Common.Client
{
    public class PowerBIClientFactory : IPowerBIClientFactory
    {
        public IPowerBIApiClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings)
        {
            return new PowerBIApiClient(authenticator, profile, logger, settings, new PowerBIHttpClientHandler(logger));
        }
    }
}
