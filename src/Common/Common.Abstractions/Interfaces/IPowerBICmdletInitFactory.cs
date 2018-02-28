/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Initialize a PowerBI Cmdlet internal property composition.
    /// </summary>
    public interface IPowerBICmdletInitFactory
    {
        /// <summary>
        /// Logger factory to use in a PowerBI cmdlet.
        /// </summary>
        IPowerBILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Data storage to use in a PowerBI cmdlet.
        /// </summary>
        IDataStorage Storage { get; }

        /// <summary>
        /// Authentication factory to use in a PowerBI cmdlet.
        /// </summary>
        IAuthenticationFactory Authenticator { get; }

        /// <summary>
        /// Default settings for a PowerBI cmdlet.
        /// </summary>
        IPowerBISettings Settings { get; }

        /// <summary>
        /// Deconstructor to pull apart the PowerBICmdletInitFactory to its various properties for populating a PowerBICmdlet.
        /// </summary>
        /// <param name="logger">Logger to use in the PowerBI cmdlet.</param>
        /// <param name="storage">Data storage to use in the PowerBI cmdlet.</param>
        /// <param name="authenticator">Authentication factory to use in the PowerBI cmdlet.</param>
        /// <param name="settings">Default settings to use in the PowerBI cmdlet.</param>
        void Deconstruct(out IPowerBILoggerFactory logger, out IDataStorage storage, out IAuthenticationFactory authenticator, out IPowerBISettings settings);
    }
}