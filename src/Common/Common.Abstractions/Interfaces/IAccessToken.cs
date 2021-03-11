/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Access token returned from authentication.
    /// </summary>
    public interface IAccessToken
    {
        /// <summary>
        /// Token string.
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Authorization header, usually in the form of "Bearer AccessToken"
        /// </summary>
        string AuthorizationHeader { get; set; }

        /// <summary>
        /// Token expiration offset.
        /// </summary>
        DateTimeOffset ExpiresOn { get; set; }

        /// <summary>
        /// Tenant ID of the user.
        /// </summary>
        string TenantId { get; set; }

        /// <summary>
        /// Displayable name of the user.
        /// </summary>
        string UserName { get; set; }
    }
}