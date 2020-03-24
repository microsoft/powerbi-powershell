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
    [Cmdlet(CmdletVerb, CmdletName)]
    [Alias("Rotate-PowerBIEncryptionKey")]
    [OutputType(typeof(EncryptionKey))]
    public class SwitchPowerBIEncryptionKey : PowerBIGetEncryptionKeyClientCmdlet 
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Switch;
        private const string AdminVariable = "SwitchPowerBIEncryptionKeyAdminVariable";

        public SwitchPowerBIEncryptionKey() : base() { }

        public SwitchPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string KeyVaultKeyUri { get; set; }
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                SessionState?.PSVariable?.Set(AdminVariable, this.Scope == PowerBIUserScope.Organization);

                var encryptionKey = GetEncryptionKey(client, keyName: Name, asAdmin: this.Scope == PowerBIUserScope.Organization);
                if (encryptionKey == null)
                {
                    return;
                }

                EncryptionKey responseEncryptionKey = null;

                if (this.Scope == PowerBIUserScope.Individual)
                {
                    responseEncryptionKey = client.Encryption.RotatePowerBIEncryptionKey(encryptionKey.Id, KeyVaultKeyUri);
                } else
                {
                    responseEncryptionKey = client.Admin.RotatePowerBIEncryptionKey(encryptionKey.Id, KeyVaultKeyUri);
                }

                this.Logger.WriteObject(responseEncryptionKey);
            }
        }
    }
}
