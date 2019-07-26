using System.Collections.Generic;
using System.Security;
using Microsoft.PowerBI.Common.Api.Gateways.Entities;
using Newtonsoft.Json;

namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    public class DatasourceCredentialDetailsFactory
    {
        public static DatasourceCredentialDetails Create(
            string              username,
            string              password,
            string              publicKey,
            CredentialType      credentialType,
            EncryptedConnection encryptedConnection,
            PrivacyLevel        privacyLevel,
            string              encryptionAlgorithm,
            bool?               skipTestConnection,
            bool?               useCallerAADIdentity,
            bool?               useCustomOAuthApp,
            bool?               useEndUserOAuth2Credentials)
        {
            return new DatasourceCredentialDetails
                   {
                       Credentials = CreateEncryptedCredentials(username, password, publicKey),
                       CredentialType = credentialType.ToString(),
                       EncryptedConnection = encryptedConnection.ToString(),
                       PrivacyLevel = privacyLevel.ToString(),
                       EncryptionAlgorithm = encryptionAlgorithm,
                       SkipTestConnection = skipTestConnection,
                       UseCallerAADIdentity = useCallerAADIdentity,
                       UseCustomOAuthApp = useCustomOAuthApp,
                       UseEndUserOAuth2Credentials = useEndUserOAuth2Credentials

            };
        }

        private static string CreateEncryptedCredentials(string username, string password, string publicKey)
        {
            var credentialData = new CredentialData
                                 {
                                     CredentialNameValuePairs = new List<CredentialNameValuePair>
                                                                {
                                                                    new CredentialNameValuePair
                                                                    {
                                                                        Name  = DataMovementConstants.UserIdKey,
                                                                        Value = username
                                                                    },
                                                                    new CredentialNameValuePair
                                                                    {
                                                                        Name  = DataMovementConstants.PasswordKey,
                                                                        Value = password
                                                                    },
                                                                },
                                 };

            return AsymmetricKeyEncryptionHelper.Encrypt(
                JsonConvert.SerializeObject(credentialData),
                publicKey);
        }
    }
}