/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(Dataset))]
    public class NewPowerBIDataset : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.New;
        public const string CmdletName = "PowerBIDataset";

        #region ParameterSets
        #endregion

        #region Parameters
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public Table[] Tables { get; set; }

        #endregion

        #region Constructors
        public NewPowerBIDataset() : base() { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            var result = new Dataset()
            {
                Name = this.Name,
                Tables = this.Tables
            };

            
            this.Logger.WriteObject(result, true);
        }
    }
}
