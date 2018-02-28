/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Security;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Abstractions
{
    public class PowerBIProfile : IPowerBIProfile
    {
        public IPowerBIEnvironment Environment { get; }

        public string TenantId { get; }

        public string UserName { get; }

        public PowerBIProfileType LoginType { get; }

        public SecureString Password { get; }

        public string Thumbprint { get; }

        public PowerBIProfile(IPowerBIEnvironment environment, IAccessToken token) =>
            (this.Environment, this.TenantId, this.UserName, this.LoginType) = (environment, token.TenantId, token.UserName, PowerBIProfileType.User);

        public PowerBIProfile(IPowerBIEnvironment environment, string userName, SecureString password, IAccessToken token) => 
            (this.Environment, this.TenantId, this.UserName, this.Password, this.LoginType) = (environment, token.TenantId, userName, password, PowerBIProfileType.ServicePrincipal);

        public PowerBIProfile(IPowerBIEnvironment environment, string clientId, string thumbprint, IAccessToken token) =>
            (this.Environment, this.TenantId, this.UserName, this.Thumbprint, this.LoginType) = (environment, token.TenantId, clientId, thumbprint, PowerBIProfileType.Certificate);
    }
}