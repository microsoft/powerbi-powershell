/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Common.Client
{
    public abstract class PowerBIClientCmdlet : PowerBICmdlet
    {
        protected IPowerBIClientFactory ClientFactory { get; set; }

        static PowerBIClientCmdlet()
        {
            var serviceCollection = GetServiceCollection();
            serviceCollection = serviceCollection
                .AddSingleton<IPowerBIClientFactory, PowerBIClientFactory>()
                .AddSingleton<IPowerBIClientCmdletInitFactory, PowerBIClientCmdletInitFactory>();
            SetProvider(serviceCollection);
        }

        public PowerBIClientCmdlet() : this(GetInstance<IPowerBIClientCmdletInitFactory>())
        {
        }

        public PowerBIClientCmdlet(IPowerBIClientCmdletInitFactory init) : base(init) => this.ClientFactory = init.Client;

        protected virtual IPowerBIClient CreateClient()
        {
            return this.ClientFactory.CreateClient(this.Authenticator, this.Profile, this.Logger, this.Settings);
        }
    }
}
