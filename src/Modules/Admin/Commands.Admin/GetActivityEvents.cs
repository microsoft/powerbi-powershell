/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.ActivityEvent;
using Microsoft.PowerBI.Common.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(ActivityEventResponse))]
    public class GetActivityEvents : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIActivityEvents";
        public const string CmdletVerb = VerbsCommon.Get;
        private const string ListParameterSetName = "List";

        public GetActivityEvents() : base() { }

        public GetActivityEvents(IPowerBIClientCmdletInitFactory init) : base(init) { }

        [Parameter(Mandatory = true, ParameterSetName = ListParameterSetName)]
        public string StartDateTime { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ListParameterSetName)]
        public string EndDateTime { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string ContinuationToken { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string Filter { get; set; }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                string formattedStartDateTime = $"'{this.StartDateTime}'";
                string formattedEndDateTime = $"'{this.EndDateTime}'";
                string formattedContinuationToken = this.ContinuationToken;
                string formattedFilter = this.Filter;
                if (!string.IsNullOrEmpty(this.ContinuationToken))
                {
                    formattedContinuationToken = $"'{this.ContinuationToken}'";
                }

                if(!string.IsNullOrEmpty(this.Filter))
                {
                    formattedFilter = $"activityType eq '{this.Filter}'";
                }

                ActivityEventResponse response = client.Admin.GetActivityEvents(formattedStartDateTime, formattedEndDateTime, formattedContinuationToken, formattedFilter);
                string jsonRepresentation = JsonConvert.SerializeObject(response);
                string indented = JValue.Parse(jsonRepresentation).ToString(Formatting.Indented);
                this.Logger.WriteObject(indented, true);
            }
        }
    }
}
