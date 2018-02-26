using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Abstractions
{
    public class PowerBICmdletInitFactory : IPowerBICmdletInitFactory
    {
        public IPowerBILoggerFactory LoggerFactory { get; }

        public IDataStorage Storage { get; }

        public IAuthenticationFactory Authenticator { get; }

        public IPowerBISettings Settings { get; }

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