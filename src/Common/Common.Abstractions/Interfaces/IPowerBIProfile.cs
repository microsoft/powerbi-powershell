/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Security;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Profile containing information for an authenticated user\principal.
    /// </summary>
    public interface IPowerBIProfile
    {
        /// <summary>
        /// PowerBI environment to authenticate and connumicate against.
        /// </summary>
        IPowerBIEnvironment Environment { get; }

        /// <summary>
        /// Tenant ID for authenticated user\principal.
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// Displayable name of user\prinicpal authenticated against.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Password of user\principal.
        /// </summary>
        SecureString Password { get; }

        /// <summary>
        /// Thumbprint of certificate for authentication.
        /// </summary>
        string Thumbprint { get; }

        /// <summary>
        /// Type of login used to create profile.
        /// </summary>
        PowerBIProfileType LoginType { get; }
    }
}