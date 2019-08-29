/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using Microsoft.PowerBI.Common.Api.ActivityEvent;
using Microsoft.PowerBI.Common.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(List<object>))]
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
        public string ActivityType { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int ResultType { get; set; }

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

            string formattedStartDateTime = $"'{this.StartDateTime}'";
            string formattedEndDateTime = $"'{this.EndDateTime}'";
            string formattedFilter = this.ActivityType;
            if(!string.IsNullOrEmpty(this.ActivityType))
            {
                formattedFilter = $"Activity eq '{this.ActivityType}'";
            }

            var finalResult = this.ExecuteCmdletHelper(formattedStartDateTime, formattedEndDateTime, formattedFilter);
            this.LogResult(finalResult);
        }

        private List<object> ExecuteCmdletHelper(string formattedStartDateTime, string formattedEndDateTime, string formattedFilter)
        {
            var finalResult = new List<object>();
            using (var client = this.CreateClient())
            {
                ActivityEventResponse response = client.Admin.GetActivityEvents(formattedStartDateTime, formattedEndDateTime, null, formattedFilter);
                while (response.ContinuationToken != null)
                {
                    finalResult = finalResult.Concat(response.ActivityEventEntities).ToList();
                    string formattedContinuationToken = $"'{WebUtility.UrlDecode(response.ContinuationToken)}'";
                    response = client.Admin.GetActivityEvents(formattedStartDateTime, formattedEndDateTime, formattedContinuationToken, formattedFilter);
                }

                finalResult = finalResult.Concat(response.ActivityEventEntities).ToList();
            }

            return finalResult;
        }

        private void LogResult(List<object> result)
        {
            switch (this.ResultType)
            {
                case (int)OutputType.JsonString:
                    {
                        string jsonRepresentation = JsonConvert.SerializeObject(result);
                        string indented = JValue.Parse(jsonRepresentation).ToString(Formatting.Indented);
                        this.Logger.WriteObject(indented, true);
                    }
                    break;
                case (int)OutputType.Objects:
                    this.Logger.WriteObject(result, true);
                    break;
                default:
                    this.Logger.ThrowTerminatingError($"{nameof(this.ResultType)} is not a valid type. Only 0 or 1 are supported.");
                    break;
            }
        }

        enum OutputType
        {
            JsonString,
            Objects
        }
    }
}
