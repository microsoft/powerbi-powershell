/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public sealed class DatasourceCredentialDetails
    {
        [DataMember(Name = "credentialType")]
        public string CredentialType { get; set; }

        [DataMember(Name = "encryptedConnection")]
        public string EncryptedConnection { get; set; }

        [DataMember(Name = "privacyLevel")]
        public string PrivacyLevel { get; set; }

        [DataMember(Name = "encryptionAlgorithm")]
        public string EncryptionAlgorithm { get; set; }

        [DataMember(Name = "credentials")]
        public string Credentials { get; set; }

        [DataMember(Name = "useCallerAADIdentity")]
        public bool? UseCallerAADIdentity { get; set; }

        [DataMember(Name = "useEndUserOAuth2Credentials")]
        public bool? UseEndUserOAuth2Credentials { get; set; }

        [DataMember(Name = "useCustomOAuthApp")]
        public bool? UseCustomOAuthApp { get; set; }

        [DataMember(Name = "skipTestConnection")]
        public bool? SkipTestConnection { get; set; }
    }
}