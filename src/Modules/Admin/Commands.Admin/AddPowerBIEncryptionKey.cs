/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
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
        private const string AdminVariable = "AddPowerBIEncryptionKeyAdminVariable";

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

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                SessionState?.PSVariable?.Set(AdminVariable, this.Scope == PowerBIUserScope.Organization);
                EncryptionKey encryptionKey = null;

                if (this.Scope == PowerBIUserScope.Individual)
                {
                    encryptionKey = client.Encryption.AddPowerBIEncryptionKey(Name, KeyVaultKeyUri, Default, Activate);

                } else
                {
                    encryptionKey = client.Admin.AddPowerBIEncryptionKey(Name, KeyVaultKeyUri, Default, Activate);
                }

                this.Logger.WriteObject(encryptionKey);
            }
        }
    }
}
