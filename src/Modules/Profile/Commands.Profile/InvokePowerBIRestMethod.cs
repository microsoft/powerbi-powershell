using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Profile
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class InvokePowerBIRestMethod : PowerBICmdlet
    {
        public const string CmdletVerb = VerbsLifecycle.Invoke;
        public const string CmdletName = "PowerBIRestMethod";

        #region Constructors
        public InvokePowerBIRestMethod() : base() { }

        public InvokePowerBIRestMethod(IPowerBICmdletInitFactory init) : base(init) { }
        #endregion

        #region Parameters
        [Parameter(Mandatory = true)]
        public string Url { get; set; }

        [Parameter(Mandatory = true)]
        public PowerBIWebRequestMethod Method { get; set; }

        [Parameter(Mandatory = false)]
        public string Body { get; set; }

        [Parameter(Mandatory = false)]
        public string Organization = "myorg";

        [Parameter(Mandatory = false)]
        public string Version = "v1.0";

        [Parameter(Mandatory = false)]
        public SwitchParameter AsObject { get; set; }
        #endregion

        public override void ExecuteCmdlet()
        {
            this.Url = $"{this.Version}/{this.Organization}/" + this.Url;
            var response = this.InvokeRestMethod(this.Url, this.Body, this.Method).Result;
            if (this.AsObject.IsPresent)
            {
                throw new NotSupportedException("TODO");
            }
            else
            {
                var result = response.Content.ReadAsStringAsync().Result;
                if (result != null)
                {
                    this.Logger.WriteObject(result);
                }
            }
        }

        private async Task<HttpResponseMessage> InvokeRestMethod(string url, string body, PowerBIWebRequestMethod requestType)
        {
            // https://msdn.microsoft.com/en-us/library/mt243842.aspx
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
            var token = this.Authenticator.Authenticate(this.Profile, this.Logger, this.Settings);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Profile.Environment.GlobalServiceEndpoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);

                HttpResponseMessage response = null;
                switch (requestType)
                {
                    case PowerBIWebRequestMethod.Get:
                        response = await client.GetAsync(url);
                        break;
                    case PowerBIWebRequestMethod.Post:
                        response = await client.PostAsync(url, new StringContent(body));
                        break;
                    case PowerBIWebRequestMethod.Delete:
                        response = await client.DeleteAsync(url);
                        break;
                    case PowerBIWebRequestMethod.Put:
                        response = await client.PutAsync(url, new StringContent(body));
                        break;
                    case PowerBIWebRequestMethod.Patch:
                        response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = new StringContent(body) });
                        break;
                    case PowerBIWebRequestMethod.Options:
                        response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, url));
                        break;
                    default:
                        throw new NotSupportedException($"{nameof(requestType)} of value {requestType} is not supported");
                }

                this.Logger.WriteVerbose($"Request Uri: {response.RequestMessage.RequestUri}");
                this.Logger.WriteVerbose($"Status Code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();
                return response;
            }
        }
    }
}
