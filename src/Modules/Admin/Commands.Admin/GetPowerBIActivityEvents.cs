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
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<object>))]
    public class GetPowerBIActivityEvents : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIActivityEvents";
        public const string CmdletVerb = VerbsCommon.Get;
        private bool validationError = false;

        public GetPowerBIActivityEvents() : base() { }

        public GetPowerBIActivityEvents(IPowerBIClientCmdletInitFactory init) : base(init) { }

        [Parameter(Mandatory = true)]
        public string StartDateTime { get; set; }

        [Parameter(Mandatory = true)]
        public string EndDateTime { get; set; }

        [Parameter(Mandatory = false)]
        public string ActivityType { get; set; }

        [Parameter(Mandatory = false)]
        public OutputType ResultType { get; set; } = OutputType.JsonString;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            try
            {
                DateTime.Parse(this.StartDateTime);
            }
            catch (Exception ex)
            {
                this.validationError = true;
                string errorId = $"{nameof(this.StartDateTime)} is not a valid DateTime.";
                var errorRecord = new ErrorRecord(ex, errorId, ErrorCategory.InvalidArgument, this.StartDateTime);
                this.Logger.ThrowTerminatingError(errorRecord);
            }

            try
            {
                DateTime.Parse(this.EndDateTime);
            }
            catch (Exception ex)
            {
                this.validationError = true;
                string errorId = $"{nameof(this.EndDateTime)} is not a valid DateTime.";
                var errorRecord = new ErrorRecord(ex, errorId, ErrorCategory.InvalidArgument, this.EndDateTime);
                this.Logger.ThrowTerminatingError(errorRecord);
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.validationError)
            {
                return;
            }

            string formattedStartDateTime = $"'{this.StartDateTime}'";
            string formattedEndDateTime = $"'{this.EndDateTime}'";
            string formattedFilter = this.ActivityType;
            if (!string.IsNullOrEmpty(this.ActivityType))
            {
                formattedFilter = $"Activity eq '{this.ActivityType}'";
            }

            var finalResult = this.ExecuteCmdletHelper(formattedStartDateTime, formattedEndDateTime, formattedFilter);
            this.LogResult(finalResult);
        }

        private IEnumerable<object> ExecuteCmdletHelper(string formattedStartDateTime, string formattedEndDateTime, string formattedFilter)
        {
            IEnumerable<object> finalResult = new List<object>();
            using (var client = this.CreateClient())
            {
                ActivityEventResponse response = client.Admin.GetPowerBIActivityEvents(formattedStartDateTime, formattedEndDateTime, null, formattedFilter);
                while (response.ContinuationToken != null)
                {
                    finalResult = finalResult.Concat(response.ActivityEventEntities);
                    string formattedContinuationToken = $"'{WebUtility.UrlDecode(response.ContinuationToken)}'";
                    response = client.Admin.GetPowerBIActivityEvents(formattedStartDateTime, formattedEndDateTime, formattedContinuationToken, formattedFilter);
                }

                finalResult = finalResult.Concat(response.ActivityEventEntities).ToList();
            }

            return finalResult;
        }

        private void LogResult(IEnumerable<object> result)
        {
            switch (this.ResultType)
            {
                case OutputType.JsonString:
                    {
                        string jsonRepresentation = JsonConvert.SerializeObject(result);
                        string indented = JValue.Parse(jsonRepresentation).ToString(Formatting.Indented);
                        this.Logger.WriteObject(indented, true);
                    }
                    break;
                case OutputType.JsonObject:
                    this.Logger.WriteObject(result, true);
                    break;
                default:
                    break;
            }
        }
    }
}
