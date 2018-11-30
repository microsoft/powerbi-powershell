/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

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
            var context = new AuthenticationContext(parsedArgs.AzureADAuthorityAddress);
            AuthenticationResult result = null;
            if(!string.IsNullOrEmpty(parsedArgs.UserName) && !string.IsNullOrEmpty(parsedArgs.Password))
            {
                // https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/wiki/Acquiring-tokens-with-username-and-password
                var passwordBytes = Convert.FromBase64String(parsedArgs.Password);
                var password = Encoding.UTF8.GetString(passwordBytes);
                // TODO encrypt password and decode here, either AES or MachineKey (but it needs to work across .NET Framework and .NET Core)
                result = await context.AcquireTokenAsync(parsedArgs.AzureADResource, parsedArgs.AzureADClientId, new UserPasswordCredential(parsedArgs.UserName, password));
            }
            else
            {
                result = await context.AcquireTokenAsync(parsedArgs.AzureADResource, parsedArgs.AzureADClientId, new Uri(parsedArgs.AzureADRedirectAddress), new PlatformParameters(PromptBehavior.Auto), UserIdentifier.AnyUser, parsedArgs.QueryParams);
            }

            var tokenCache = context.TokenCache.Serialize();
            return Convert.ToBase64String(tokenCache);
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
