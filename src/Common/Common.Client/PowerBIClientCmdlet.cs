/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Api;

namespace Microsoft.PowerBI.Common.Client
{
    public abstract class PowerBIClientCmdlet : PowerBICmdlet
    {
        protected IPowerBIClientFactory ClientFactory { get; set; }

        public PowerBIClientCmdlet() : this(GetDefaultClientInitFactory())
        {
        }

        protected static IPowerBIClientCmdletInitFactory GetDefaultClientInitFactory() => new PowerBIClientCmdletInitFactory(new PowerBILoggerFactory(), new ModuleDataStorage(), new AuthenticationFactorySelector(), new PowerBISettings(), new PowerBIClientFactory());

        public PowerBIClientCmdlet(IPowerBIClientCmdletInitFactory init) : base(init) => this.ClientFactory = init.Client;

        protected virtual IPowerBIApiClient CreateClient()
        {
            return this.ClientFactory.CreateClient(this.Authenticator, this.Profile, this.Logger, this.Settings);
        }
    }
}
