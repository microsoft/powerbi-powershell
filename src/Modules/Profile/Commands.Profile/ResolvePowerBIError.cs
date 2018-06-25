/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Commands.Profile.Errors;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Profile
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = AnyErrorParameterSet)]
    public class ResolvePowerBIError : PowerBICmdlet
    {
        #region Cmdlet name
        public const string CmdletName = "PowerBIError";
        public const string CmdletVerb = VerbsDiagnostic.Resolve;
        #endregion

        #region ParameterSets
        public const string AnyErrorParameterSet = "AnyError";
        public const string LastErrorParameterSet = "LastError";
        #endregion

        #region Parameters
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = AnyErrorParameterSet)]
        public ErrorRecord[] Error { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = LastErrorParameterSet)]
        public SwitchParameter Last { get; set; }
        #endregion

        #region Constructors
        public ResolvePowerBIError() : base() { }

        public ResolvePowerBIError(IPowerBICmdletInitFactory init) : base(init) { }
        #endregion

        protected override bool CmdletManagesProfile { get => true; set => base.CmdletManagesProfile = value; }

        public override void ExecuteCmdlet()
        {
            var analyzeErrors = new List<ErrorRecord>();
            if (this.Last.IsPresent)
            {
                var errors = GetErrorsFromVariable();
                if (errors != null)
                {
                    var error = errors.FirstOrDefault();
                    if (error != null)
                    {
                        analyzeErrors.Add(error);
                    }
                }
            }
            else
            {
                var errors = this.Error ?? GetErrorsFromVariable();
                if (errors != null)
                {
                    analyzeErrors.AddRange(errors);
                }
            }

            foreach (var error in analyzeErrors)
            {
                this.DisplayErrorRecord(error as ErrorRecord);
            }
        }

        private IEnumerable<ErrorRecord> GetErrorsFromVariable()
        {
            if (GetVariableValue("global:Error", null) is IEnumerable errors)
            {
                return errors.OfType<ErrorRecord>();
            }

            return null;
        }

        private void DisplayErrorRecord(ErrorRecord record)
        {
            if (record == null)
            {
                return;
            }

            if (record.Exception != null)
            {
                DisplayException(record.Exception, record);
            }
            else
            {
                this.Logger.WriteObject(new PowerBIErrorRecord(record));
            }
        }

        private void DisplayException(Exception exception, ErrorRecord record, bool inner = false)
        {
            if (exception is AggregateException aggregate)
            {
                foreach (var innerException in aggregate.InnerExceptions.Where(e => e != null))
                {
                    DisplayException(innerException, record, true);
                }
            }
            else if (exception is Microsoft.Rest.HttpOperationException restException)
            {
                this.Logger.WriteObject(new PowerBIRestExceptionRecord(restException, record, inner));
            }
            else
            {
                this.Logger.WriteObject(new PowerBIExceptionRecord(exception, record, inner));
                if(exception.InnerException != null)
                {
                    DisplayException(exception.InnerException, record, true);
                }
            }
        }
    }
}
