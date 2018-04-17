/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Commands.Common;

namespace Microsoft.PowerBI.Commands.Profile
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [Alias("Logout-PowerBIServiceAccount", "Remove-PowerBIServiceAccount")]
    public class DisconnectPowerBIServiceAccount : PowerBICmdlet
    {
        public const string CmdletVerb = VerbsCommunications.Disconnect;
        public const string CmdletName = "PowerBIServiceAccount";

        public override void ExecuteCmdlet()
        {
            if (this.Profile != null)
            {
                this.Storage.RemoveItem("profile");
            }

            this.Authenticator.Challenge();
        }

        protected override bool CmdletManagesProfile { get => true; set => base.CmdletManagesProfile = value; }
    }
}