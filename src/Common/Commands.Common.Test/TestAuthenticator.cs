/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Authentication;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestAuthenticator : IAuthenticationFactory
    {
        public bool AuthenticatedOnce { get; set; } = true;

        public IAccessToken Token { get; set; }

        public TestAuthenticator(IAccessToken token = null)
        {
            string stringToken = token == null ? TestUtilities.GetRandomString(255) : null;
            this.Token = token ?? new PowerBIAccessToken()
            {
                AccessToken = stringToken,
                AuthorizationHeader = $"Bearer {stringToken}",
                AccessTokenType = "Bearer",
                UserName = "user1@contoso.com",
                Authority = "https://fake.net/",
                ExpiresOn = DateTime.UtcNow.AddHours(1),
                TenantId = Guid.NewGuid().ToString()
            };
        }

        public IAccessToken Authenticate(IPowerBIProfile profile, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            return this.Token;
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, IDictionary<string, string> queryParameters = null)
        {
            return this.Token;
        }

        public IAccessToken Authenticate(IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings, string userName, SecureString password)
        {
            return this.Token;
        }

        public IAccessToken Authenticate(string userName, SecureString password, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            return this.Token;
        }

        public IAccessToken Authenticate(string clientId, string thumbprint, IPowerBIEnvironment environment, IPowerBILogger logger, IPowerBISettings settings)
        {
            return this.Token;
        }

        public Task Challenge(ICollection<IPowerBIEnvironment> environments)
        {
            // Nothing
            return Task.Delay(1);
        }
    }
}
