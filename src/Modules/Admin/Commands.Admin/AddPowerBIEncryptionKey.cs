/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DefaultAndActivateParameterSet)]
    [OutputType(typeof(EncryptionKey))]
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
