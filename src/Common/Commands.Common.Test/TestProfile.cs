/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common.Test
{
    public class TestProfile : IPowerBIProfile
    {
        public IPowerBIEnvironment Environment { get; set; } = new PowerBIEnvironment()
        {
            AzureADAuthority = "https://fake.net/common/oauth2/authorize",
            AzureADClientId = Guid.NewGuid().ToString(),
            AzureADRedirectAddress = "urn:fake",
            AzureADResource = "https://fake.net/powerbi/api",
            GlobalServiceEndpoint = "https://fake.api.powerbi.com",
            Name = PowerBIEnvironmentType.Public
        };

        public string TenantId { get; set; } = Guid.NewGuid().ToString();

        public string UserName { get; set; } = "user1@contoso.com";

        public SecureString Password { get; set; }

        public string Thumbprint { get; set; }

        public PowerBIProfileType LoginType { get; set; } = PowerBIProfileType.User;
    }
}
