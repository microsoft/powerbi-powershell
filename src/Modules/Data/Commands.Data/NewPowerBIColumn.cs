/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(Column))]
    public class NewPowerBIColumn : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.New;
        public const string CmdletName = "PowerBIColumn";

        #region ParameterSets
        #endregion

        #region Parameters
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public PowerBIDataType DataType { get; set; }

        #endregion

        #region Constructors
        public NewPowerBIColumn() : base() { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            var result = new Column()
            {
                Name = this.Name,
                DataType = this.DataType.ToString()
            };

            
            this.Logger.WriteObject(result, true);
        }
    }
}
