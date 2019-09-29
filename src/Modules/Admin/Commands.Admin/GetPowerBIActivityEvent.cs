/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using Microsoft.PowerBI.Commands.Profile.Errors;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.ActivityEvent;
using Microsoft.PowerBI.Common.Client;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [Alias("Get-PowerBIActivityEvents")]
    [OutputType(typeof(IList<object>))]
    public class GetPowerBIActivityEvent : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIActivityEvent";
        public const string CmdletVerb = VerbsCommon.Get;
        private const string FeatureNotAvailableError = "FeatureNotAvailableError";
        private bool validationError = false;
        private string Filter = null;
        private string ActivityTypeFilter = null;
        private string UserIdFilter = null;

        public GetPowerBIActivityEvent() : base() { }

        public GetPowerBIActivityEvent(IPowerBIClientCmdletInitFactory init) : base(init) { }

        [Parameter(Mandatory = true)]
        public string StartDateTime { get; set; }

        [Parameter(Mandatory = true)]
        public string EndDateTime { get; set; }

        [Parameter(Mandatory = false)]
        public string ActivityType
        {
            get => this.ActivityTypeFilter;
            set => this.ActivityTypeFilter = !string.IsNullOrEmpty(value) ? $"Activity eq '{value}'" : value;
        }

        [Parameter(Mandatory = false)]
        public string User
        {
            get => this.UserIdFilter;
            set => this.UserIdFilter = !string.IsNullOrEmpty(value) ? $"UserId eq '{value}'" : value;
        }

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
            if (!string.IsNullOrEmpty(this.ActivityType))
            {
                this.Filter = string.IsNullOrEmpty(this.Filter) ? this.ActivityType : $"({this.Filter}) and ({this.ActivityType})";
            }
            if (!string.IsNullOrEmpty(this.User))
            {
                this.Filter = string.IsNullOrEmpty(this.Filter) ? this.User : $"({this.Filter}) and ({this.User})";
            }

            var finalResult = this.ExecuteCmdletHelper(formattedStartDateTime, formattedEndDateTime);
            this.LogResult(finalResult);
        }

        private IList<object> ExecuteCmdletHelper(string formattedStartDateTime, string formattedEndDateTime)
        {
            var finalResult = new List<object>();
            using (var client = this.CreateClient())
            {
                try
                {
                    ActivityEventResponse response = client.Admin.GetPowerBIActivityEvents(formattedStartDateTime, formattedEndDateTime, null, this.Filter);
                    while (response.ContinuationToken != null)
                    {
                        finalResult.AddRange(response.ActivityEventEntities);
                        string formattedContinuationToken = $"'{WebUtility.UrlDecode(response.ContinuationToken)}'";
                        response = client.Admin.GetPowerBIActivityEvents(null, null, formattedContinuationToken, null);
                    }

                    finalResult.AddRange(response.ActivityEventEntities);
                }
                catch (HttpOperationException ex)
                {
                    var deserialized = JsonConvert.DeserializeObject<PowerBIFeatureNotAvailableErrorType>(ex?.Response?.Content);
                    if (deserialized != null && deserialized.Error != null && deserialized.Error.Code.Equals(FeatureNotAvailableError))
                    {
                        string errorId = "Feature is not available on your tenant.";
                        var errorRecord = new ErrorRecord(ex, errorId, ErrorCategory.NotEnabled, this /*targetObject*/);
                        var powerBIRestExceptionRecord = new PowerBIRestExceptionRecord(ex, errorRecord);
                        this.Logger.ThrowTerminatingError(powerBIRestExceptionRecord, ErrorCategory.NotEnabled);
                    }
                    else
                    {
                        throw;
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
