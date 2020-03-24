/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<EncryptionKey>))]
    public class GetPowerBIEncryptionKey : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Get;
        private const string AdminVariable = "GetPowerBIEncryptionKeyAdminVariable";

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        public GetPowerBIEncryptionKey() : base() { }

        public GetPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init): base(init) { }

        public override void ExecuteCmdlet()
        {
            SessionState?.PSVariable?.Set(AdminVariable, this.Scope == PowerBIUserScope.Organization);

            using (var client = this.CreateClient())
            {
                IEnumerable<EncryptionKey> encryptionKey = null;

                if (this.Scope == PowerBIUserScope.Individual)
                {
                    encryptionKey = client.Encryption.GetPowerBIEncryptionKeys();

                } else
                {
                    encryptionKey = client.Admin.GetPowerBIEncryptionKeys();
                }

                this.Logger.WriteObject(encryptionKey, enumerateCollection: true);
            }
        }
    }
}
