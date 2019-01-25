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
    public class GetPowerBIEncryptionKeys : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKeys";
        public const string CmdletVerb = VerbsCommon.Get;

        public GetPowerBIEncryptionKeys() : base() { }

        public GetPowerBIEncryptionKeys(IPowerBIClientCmdletInitFactory init): base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                try
                {
                    var response = client.Admin.GetPowerBIEncryptionKeys();
                    this.Logger.WriteObject(response, true);
                }
                catch (Exception ex)
                {
                    this.Logger.ThrowTerminatingError(ex);
                }
            }
        }
    }
}
