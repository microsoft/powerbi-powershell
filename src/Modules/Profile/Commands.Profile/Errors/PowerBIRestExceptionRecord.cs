using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Profile.Errors
{
    public class PowerBIRestExceptionRecord : PowerBIExceptionRecord
    {
        public PowerBIRestExceptionRecord(HttpOperationException exception, ErrorRecord record, bool inner = false) : base(exception, record, inner)
        {
            if(exception == null)
            {
                return;
            }

            if (exception.Response != null)
            {
                this.Response = $"{exception.Response.ReasonPhrase} ({(int)exception.Response.StatusCode}): {exception.Response.Content ?? "<< NO CONTENT >>"}";
                this.PowerBIErrorInfo = this.GetResponseHeaderInfo(exception.Response, "X-PowerBI-Error-Info");
                this.RequestId = this.GetResponseHeaderInfo(exception.Response, "RequestId");
                this.ResponseDate = this.GetResponseHeaderInfo(exception.Response, "Date");
            }

            if(exception.Request != null)
            {
                this.RequestMethod = exception.Request.Method.ToString();
                this.RequestUri = exception.Request.RequestUri.ToString();
            }
        }

        private string GetResponseHeaderInfo(HttpResponseMessageWrapper response, string headerName)
        {
            if(response == null || response.Headers == null)
            {
                return null;
            }

            if (response.Headers.ContainsKey(headerName))
            {
                var header = response.Headers[headerName].Select(e => e.ToString());
                if (header != null)
                {
                    var headerStringBuilder = new StringBuilder();
                    foreach (var info in header)
                    {
                        headerStringBuilder.AppendLine(info);
                    }

                    return headerStringBuilder.ToString().TrimEnd();
                }
            }

            return null;
        }

        public string Response { get; set; }
        public string PowerBIErrorInfo { get; set; }
        public string RequestId { get; set; }
        public string ResponseDate { get; set; }
        public string RequestMethod { get; set; }
        public string RequestUri { get; set; }
    }
}
