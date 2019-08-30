/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using Microsoft.PowerBI.Commands.Profile.Errors;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.ActivityEvent;
using Microsoft.PowerBI.Common.Client;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IList<object>))]
    public class GetPowerBIActivityEvents : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIActivityEvents";
        public const string CmdletVerb = VerbsCommon.Get;
        private const string FeatureNotAvailableError = "FeatureNotAvailableError";
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

        private IList<object> ExecuteCmdletHelper(string formattedStartDateTime, string formattedEndDateTime, string formattedFilter)
        {
            var finalResult = new List<object>();
            using (var client = this.CreateClient())
            {
                try
                {
                    ActivityEventResponse response = client.Admin.GetPowerBIActivityEvents(formattedStartDateTime, formattedEndDateTime, null, formattedFilter);
                    while (response.ContinuationToken != null)
                    {
                        finalResult.AddRange(response.ActivityEventEntities);
                        string formattedContinuationToken = $"'{WebUtility.UrlDecode(response.ContinuationToken)}'";
                        response = client.Admin.GetPowerBIActivityEvents(formattedStartDateTime, formattedEndDateTime, formattedContinuationToken, formattedFilter);
                    }

                    finalResult.AddRange(response.ActivityEventEntities);
                }
                catch (HttpOperationException ex)
                {
                    var deserialized = JsonConvert.DeserializeObject<PowerBIFeatureNotAvailableErrorType>(ex?.Response?.Content);
                    if (deserialized != null && deserialized.Error != null && deserialized.Error.Code.Equals(FeatureNotAvailableError))
                    {
                        string errorId = "Feature is not available on your tenant.";
                        var errorRecord = new ErrorRecord(ex, errorId, ErrorCategory.NotEnabled, null /*targetObject*/);
                        var powerBIRestExceptionRecord = new PowerBIRestExceptionRecord(ex, errorRecord);
                        this.Logger.ThrowTerminatingError(powerBIRestExceptionRecord, ErrorCategory.NotEnabled);
                    }
                }
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
