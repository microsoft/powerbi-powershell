/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace AzureADWindowsAuthenticator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var parsedArgs = ParseArguments<ParsedArguments>(args);
            var tokenCacheString = GetToken(parsedArgs).Result;
            Console.Write(tokenCacheString);
        }

        private static async Task<string> GetToken(ParsedArguments parsedArgs)
        {
            IEnumerable<string> scopes = new[] { $"{parsedArgs.AzureADResource}/.default" };
            IPublicClientApplication app = PublicClientApplicationBuilder
                .Create(parsedArgs.AzureADClientId)
                .WithAuthority(parsedArgs.AzureADAuthorityAddress)
                .Build();
            AuthenticationResult result = null;
            if (!string.IsNullOrEmpty(parsedArgs.UserName) && parsedArgs.Password != null && parsedArgs.Password.Length > 0)
            {
                // https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/wiki/Acquiring-tokens-with-username-and-password
                result = await app.AcquireTokenByUsernamePassword(scopes, parsedArgs.UserName, parsedArgs.Password).ExecuteAsync();
            }
            else
            {
                result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
            }

            return result.AccessToken;
        }

        private static T ParseArguments<T>(string[] args) where T : new()
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Missing arguments");
            }

            var result = new T();
            var parameterTypes = typeof(T).GetProperties().Select(p => {
                var attribute = p.GetCustomAttributes(typeof(ParameterAttribute), false).Cast<ParameterAttribute>().Single();
                return new
                {
                    TypeName = p.Name,
                    Property = p,
                    ParameterName = attribute.Name,
                    Mandatory = attribute.Mandatory
                };
            });

            for (int i = 0; i < args.Length; i++)
            {
                var portions = args[i].Split(new[] { ':' }, 2);
                if (portions?.Length != 2)
                {
                    throw new ArgumentException("Argument was not in the form of -property:\"value\"", nameof(args));
                }

                var argName = portions[0].TrimStart('-');
                var argValue = portions[1].Trim('"');
                var paramType = parameterTypes.FirstOrDefault(p => p.ParameterName.Equals(argName, StringComparison.OrdinalIgnoreCase));
                if (paramType == null)
                {
                    throw new ArgumentException($"Invalid property '{argName}', supported parameters: {string.Join(", ", parameterTypes.Select(p => p.ParameterName))}", nameof(args));
                }

                paramType.Property.SetValue(result, argValue);
            }

            // Validate no mandatory parameters are missing
            var mandatoryParams = parameterTypes.Where(p => p.Mandatory);
            foreach (var mandatoryParam in mandatoryParams)
            {
                if (mandatoryParam.Property.GetValue(result) == null)
                {
                    throw new ArgumentException($"Mandatory parameter -{mandatoryParam.ParameterName} is missing", nameof(args));
                }
            }

            return result;
        }
    }
}
