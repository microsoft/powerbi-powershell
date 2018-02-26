using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public static class Extensions
    {
        public static string ToQueryParameterString(this IDictionary<string, string> queryParameters)
        {
            string queryParamString = null;
            if(queryParameters != null)
            {
                queryParamString = string.Join("&", queryParameters.Select(q => $"{q.Key}={q.Value}"));
            }

            return queryParamString;
        }

        public static IAccessToken ToIAccessToken(this AuthenticationResult result)
        {
            if(result == null)
            {
                return null;
            }

            return new PowerBIAccessToken()
            {
                AccessToken = result.AccessToken,
                AccessTokenType = result.AccessTokenType,
                Authority = result.Authority,
                ExpiresOn = result.ExpiresOn,
                TenantId = result.TenantId,
                UserName = result.UserInfo?.DisplayableId,
                AuthorizationHeader = result.CreateAuthorizationHeader()
            };
        }
    }
}