/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DefaultParameterSet)]
    [OutputType(typeof(TenantKey))]
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

        [Parameter(ParameterSetName = DefaultParameterSet, Mandatory = true)]
        [Parameter(ParameterSetName = DefaultAndActivateParameterSet, Mandatory = true)]
        public SwitchParameter Default { get; set; }

        [Parameter(ParameterSetName = ActivateParameterSet, Mandatory = true)]
        [Parameter(ParameterSetName = DefaultAndActivateParameterSet, Mandatory = true)]
        public SwitchParameter Activate { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            this.Logger.WriteWarning(Constants.PrivatePreviewWarning);
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var response = client.Admin.AddPowerBIEncryptionKey(Name, KeyVaultKeyUri, Default, Activate);
                this.Logger.WriteObject(response);
            }
        }
    }
}
