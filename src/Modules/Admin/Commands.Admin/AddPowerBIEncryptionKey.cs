/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class AddPowerBIEncryptionKey : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Add;

        public AddPowerBIEncryptionKey() : base() { }

        public AddPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string KeyVaultKeyUri { get; set; }

        [Parameter(Mandatory = false)]
        public bool Default { get; set; } = false;

        [Parameter(Mandatory = false)]
        public bool Activate { get; set; } = false;

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
                try
                {
                    var response = client.Admin.AddPowerBIEncryptionKey(Name, KeyVaultKeyUri, Default, Activate);
                    this.Logger.WriteObject(response);
                }
                catch (Exception ex)
                {
                    this.Logger.ThrowTerminatingError(ex);
                }
            }
        }

        private void ThrowNotSupportedException(string parameterName, object parameterValue)
        {
            var notSupportedException = new NotSupportedException($"{parameterName} value {parameterValue} is not yet supported");
            this.Logger.ThrowTerminatingError(notSupportedException, ErrorCategory.InvalidArgument);
        }
    }
}
