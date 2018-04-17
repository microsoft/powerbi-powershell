/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Client
{
    public interface IPowerBIClientFactory
    {
        IPowerBIClient CreateClient(IAuthenticationFactory authenticator, IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings);
    }
}
