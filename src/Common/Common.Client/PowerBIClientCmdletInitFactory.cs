/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Client
{
    public class PowerBIClientCmdletInitFactory : PowerBICmdletInitFactory, IPowerBIClientCmdletInitFactory
    {
        public IPowerBIClientFactory Client { get; set; }

        public PowerBIClientCmdletInitFactory(IPowerBILoggerFactory logger, IDataStorage storage, IAuthenticationFactory authenticator, IPowerBISettings settings, IPowerBIClientFactory client) : base(logger, storage, authenticator, settings) => this.Client = client;
    }
}
