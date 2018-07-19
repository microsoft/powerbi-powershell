/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Api.Datasets;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(Table))]
    public class NewPowerBITable : PowerBICmdlet
    {
        public const string CmdletVerb = VerbsCommon.New;
        public const string CmdletName = "PowerBITable";

        #region ParameterSets
        #endregion

        #region Parameters
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public Column[] Columns { get; set; }

        #endregion

        #region Constructors
        public NewPowerBITable() : base() { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            var result = new Table()
            {
                Name = this.Name,
                Columns = this.Columns
            };

            
            this.Logger.WriteObject(result, true);
        }
    }
}
