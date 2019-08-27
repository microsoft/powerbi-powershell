/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using System.Net;
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
        private bool validationError = false;

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

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            try
            {
                DateTime.Parse(this.StartDateTime);
            }
            catch
            {
                this.validationError = true;
                this.Logger.ThrowTerminatingError($"{nameof(this.StartDateTime)} is not a valid DateTime.");
            }

            try
            {
                DateTime.Parse(this.EndDateTime);
            }
            catch
            {
                this.validationError = true;
                this.Logger.ThrowTerminatingError($"{nameof(this.EndDateTime)} is not a valid DateTime.");
            }
        }

        public override void ExecuteCmdlet()
        {
            if(this.validationError)
            {
                return;
            }

            using (var client = this.CreateClient())
            {
                string formattedStartDateTime = $"'{this.StartDateTime}'";
                string formattedEndDateTime = $"'{this.EndDateTime}'";
                string formattedContinuationToken = this.ContinuationToken;
                string formattedFilter = this.Filter;
                if (!string.IsNullOrEmpty(this.ContinuationToken))
                {
                    formattedContinuationToken = $"'{WebUtility.UrlDecode(this.ContinuationToken)}'";
                }

                if(!string.IsNullOrEmpty(this.Filter))
                {
                    formattedFilter = $"Activity eq '{this.Filter}'";
                }

                ActivityEventResponse response = client.Admin.GetActivityEvents(formattedStartDateTime, formattedEndDateTime, formattedContinuationToken, formattedFilter);
                string jsonRepresentation = JsonConvert.SerializeObject(response);
                string indented = JValue.Parse(jsonRepresentation).ToString(Formatting.Indented);
                this.Logger.WriteObject(indented, true);
            }
        }
    }
}
