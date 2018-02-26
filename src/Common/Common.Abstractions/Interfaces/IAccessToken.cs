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
        /// Secure token service authority.
        /// </summary>
        string Authority { get; set; }

        /// <summary>
        /// Token expiration offset.
        /// </summary>
        DateTimeOffset ExpiresOn { get; set; }

        /// <summary>
        /// Tenant ID of the user.
        /// </summary>
        string TenantId { get; set; }

        /// <summary>
        /// Type of access token.
        /// </summary>
        string AccessTokenType { get; set; }

        /// <summary>
        /// Displayable name of the user.
        /// </summary>
        string UserName { get; set; }
    }
}