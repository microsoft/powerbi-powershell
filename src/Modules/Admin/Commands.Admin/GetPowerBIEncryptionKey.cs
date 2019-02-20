/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Management.Automation;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class GetPowerBIEncryptionKey : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Get;

        public GetPowerBIEncryptionKey() : base() { }

        public GetPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init): base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var response = client.Admin.GetPowerBIEncryptionKeys();
                this.Logger.WriteObject(response, true);
            }
        }
    }
}
