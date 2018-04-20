/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestPowerBICmdletInitFactory : PowerBIClientCmdletInitFactory
    {
        public IPowerBIProfile Profile { get; set; }

        public TestLogger Logger
        {
            get
            {
                if(this.LoggerFactory is TestLoggerFactory logger)
                {
                    return logger.Logger;
                }

                return null;
            }
        }

        public TestPowerBICmdletInitFactory(IPowerBIApiClient client) :
            base(new TestLoggerFactory(), new ModuleDataStorage(), new TestAuthenticator(), new PowerBISettings(), new TestClient(client)) => this.SetProfile();

        public TestPowerBICmdletInitFactory(FakeHttpClientHandler clientHandler) :
            base(new TestLoggerFactory(), new ModuleDataStorage(), new TestAuthenticator(), new PowerBISettings(), new TestClient(clientHandler)) => this.SetProfile();

        private void SetProfile(IPowerBIProfile profile = null)
        {
            if (profile == null)
            {
                this.Profile = new TestProfile();
            }
            else if(this.Profile != profile)
            {
                this.Profile = profile;
            }

            this.Storage.SetItem("profile", this.Profile);
        }
    }
}
