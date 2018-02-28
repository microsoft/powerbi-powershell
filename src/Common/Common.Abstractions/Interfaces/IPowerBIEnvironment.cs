/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// PowerBI Environment containing authentication and communication properties.
    /// </summary>
    public interface IPowerBIEnvironment
    {
        /// <summary>
        /// Name of the PowerBI environment.
        /// </summary>
        PowerBIEnvironmentType Name { get; set; }

        /// <summary>
        /// Azure Active Directory Secure Token Service (STS) Authority.
        /// </summary>
        string AzureADAuthority { get; set; }
        
        /// <summary>
        /// Azure Active Directory Resource to authenticate against..
        /// </summary>
        string AzureADResource { get; set; }

        /// <summary>
        /// Azure Active Directory (AAD) ClientId for the AAD application.
        /// </summary>
        string AzureADClientId { get; set; }

        /// <summary>
        /// Azure Active Directory (AAD) Redirect Address for AAD application.
        /// </summary>
        string AzureADRedirectAddress { get; set; }

        /// <summary>
        /// Default endpoint to communicate with the PowerBI global service.
        /// </summary>
        string GlobalServiceEndpoint { get; set; }

        /// <summary>
        /// Override to communicate with the PowerBI service.
        /// </summary>
        string BackendUriOverride { get; set; }
    }
}