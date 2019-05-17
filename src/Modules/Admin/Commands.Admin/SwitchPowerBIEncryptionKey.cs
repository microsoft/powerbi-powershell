/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [Alias("Rotate-PowerBIEncryptionKey")]
    [OutputType(typeof(EncryptionKey))]
    public class SwitchPowerBIEncryptionKey : PowerBIAdminClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Switch;

        public SwitchPowerBIEncryptionKey() : base() { }

        public SwitchPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string KeyVaultKeyUri { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var encryptionKey = GetEncryptionKey(client, keyName: Name);
                if (encryptionKey == null)
                {
                    return;
                }

                var response = client.Admin.RotatePowerBIEncryptionKey(encryptionKey.Id, KeyVaultKeyUri);
                this.Logger.WriteObject(response);
            }
        }
    }
}
