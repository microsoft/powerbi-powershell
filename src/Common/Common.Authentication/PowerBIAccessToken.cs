/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public class PowerBIAccessToken : IAccessToken
    {
        public string AccessToken { get; set; }
        public string AuthorizationHeader { get; set; }
        public string Authority { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public string TenantId { get; set; }
        public string AccessTokenType { get; set; }
        public string UserName { get; set; }
    }
}