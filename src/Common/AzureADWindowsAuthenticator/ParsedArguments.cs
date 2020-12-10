/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Security;

namespace AzureADWindowsAuthenticator
{
    public class ParsedArguments
    {
        [Parameter("Authority", Mandatory = true)]
        public string AzureADAuthorityAddress { get; set; }

        [Parameter("Resource", Mandatory = true)]
        public string AzureADResource { get; set; }

        [Parameter("ID", Mandatory = true)]
        public string AzureADClientId { get; set; }

        [Parameter("Redirect", Mandatory = true)]
        public string AzureADRedirectAddress { get; set; }

        [Parameter("Query")]
        public string QueryParams { get; set; }

        [Parameter("PW")]
        public SecureString Password { get; set; }

        [Parameter("User")]
        public string UserName { get; set; }
    }
}
