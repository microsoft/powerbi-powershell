using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class GetPowerBIDataset : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIDataset";

        #region Parameters
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;
        #endregion

        public override void ExecuteCmdlet()
        {
            
        }
    }
}
