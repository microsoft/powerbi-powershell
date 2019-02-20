/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DefaultParameterSet)]
    public class AddPowerBIEncryptionKey : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Add;

        public AddPowerBIEncryptionKey() : base() { }

        public AddPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameter set names
        public const string DefaultParameterSet = "Default";
        public const string ActivateParameterSet = "Activate";
        public const string DefaultAndActivateParameterSet = "DefaultAndActivate";
        #endregion

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string KeyVaultKeyUri { get; set; }

        [Parameter(ParameterSetName = DefaultParameterSet, Mandatory = false)]
        [Parameter(ParameterSetName = DefaultAndActivateParameterSet, Mandatory = false)]
        public SwitchParameter Default { get; set; }

        [Parameter(ParameterSetName = ActivateParameterSet, Mandatory = false)]
        [Parameter(ParameterSetName = DefaultAndActivateParameterSet, Mandatory = false)]
        public SwitchParameter Activate { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.Default == false)
            {
                this.ThrowNotSupportedException(nameof(this.Default), this.Default);
            }

            if (this.Activate == false)
            {
                this.ThrowNotSupportedException(nameof(this.Activate), this.Activate);
            }
        }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var response = client.Admin.AddPowerBIEncryptionKey(Name, KeyVaultKeyUri, Default, Activate);
                this.Logger.WriteObject(response);
            }
        }

        private void ThrowNotSupportedException(string parameterName, object parameterValue)
        {
            var notSupportedException = new NotSupportedException($"Parameter {parameterName} with value {parameterValue} is not supported");
            this.Logger.ThrowTerminatingError(notSupportedException, ErrorCategory.InvalidArgument);
        }
    }
}
