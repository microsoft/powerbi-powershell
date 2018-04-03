/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Abstractions
{
    public class PowerBICmdletInitFactory : IPowerBICmdletInitFactory
    {
        public IPowerBILoggerFactory LoggerFactory { get; set; }

        public IDataStorage Storage { get; set; }

        public IAuthenticationFactory Authenticator { get; set; }

        public IPowerBISettings Settings { get; set; }

        public PowerBICmdletInitFactory(IPowerBILoggerFactory logger, IDataStorage storage, IAuthenticationFactory authenticator, IPowerBISettings settings)
            => (this.LoggerFactory, this.Storage, this.Authenticator, this.Settings) = (logger, storage, authenticator, settings);

        public void Deconstruct(out IPowerBILoggerFactory logger, out IDataStorage storage, out IAuthenticationFactory authenticator, out IPowerBISettings settings)
        {
            logger = this.LoggerFactory;
            storage = this.Storage;
            authenticator = this.Authenticator;
            settings = this.Settings;
        }
    }
}