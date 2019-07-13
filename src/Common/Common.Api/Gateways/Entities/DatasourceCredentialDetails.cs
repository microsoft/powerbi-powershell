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
        [DataMember(Name = "credentialType", EmitDefaultValue = false)]
        public string CredentialType { get; set; }

        [DataMember(Name = "encryptedConnection", EmitDefaultValue = false)]
        public string EncryptedConnection { get; set; }

        [DataMember(Name = "privacyLevel", EmitDefaultValue = false)]
        public string PrivacyLevel { get; set; }

        [DataMember(Name = "encryptionAlgorithm", EmitDefaultValue = false)]
        public string EncryptionAlgorithm { get; set; }

        [DataMember(Name = "credentials", EmitDefaultValue = false)]
        public string Credentials { get; set; }

        [DataMember(Name = "useCallerAADIdentity", EmitDefaultValue = false)]
        public bool? UseCallerAADIdentity { get; set; }

        [DataMember(Name = "useEndUserOAuth2Credentials", EmitDefaultValue = false)]
        public bool? UseEndUserOAuth2Credentials { get; set; }

        [DataMember(Name = "useCustomOAuthApp", EmitDefaultValue = false)]
        public bool? UseCustomOAuthApp { get; set; }

        [DataMember(Name = "skipTestConnection", EmitDefaultValue = false)]
        public bool? SkipTestConnection { get; set; }

        [DataMember(Name = "isCredentialEncrypted", EmitDefaultValue = false)]
        public bool IsCredentialEncrypted { get; }
    }
}