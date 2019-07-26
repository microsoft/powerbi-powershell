namespace Microsoft.PowerBI.Common.Api.Gateways.Credentials
{
    public static class DataMovementConstants
    {
        public const string AESEncryptionAlgorithm      = "AES";
        public const string RSA_OAEPEncryptionAlgorithm = "RSA-OAEP";
        public const string NoneEncryptionAlgorithm     = "NONE";

        public const string UserIdKey             = "username";
        public const string PasswordKey           = "password";
        public const string CredentialKey         = "key";
        public const string OAuthAccessToken      = "AccessToken";
        public const string OAuthExpires          = "Expires";
        public const string OAuthRefreshToken     = "RefreshToken";
        public const string OAuthPropertyPrefix   = "Property-";
        public const string AccessTokenPrefix     = "AccessToken:";
        public const string OAuthTokenType        = "token_type";
        public const string OAuthOAuth2Nonce      = "oAuth2Nonce";
        public const string OAuthRedirectEndpoint = "redirectEndpoint";

        public const string CustomOAuthAppId              = "AppId";
        public const string CustomOAuthIdentityType       = "IdentityType";
        public const string CustomOAuthSecretUrl          = "SecretUrl";
        public const string CustomOAuthDatasourceTenantId = "TenantId";
        public const string CustomOAuthFakeAccessToken    = "FakeAccessTokenForUpdateCredentialByRefreshTokenFlow";
    }
}
