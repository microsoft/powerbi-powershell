/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DatasetParameterSetName)]
    [OutputType(typeof(Dataset))]
    public class AddPowerBIDataset : PowerBIClientCmdlet, IUserScope
    {
        public const string CmdletVerb = VerbsCommon.Add;
        public const string CmdletName = "PowerBIDataset";

        #region ParameterSets
        private const string DatasetParameterSetName = "Dataset";
        private const string WorkspaceParameterSetName = "Workspace";
        #endregion

        #region Parameters
        [Parameter(Mandatory = true)]
        public Dataset Dataset { get; set; }
       
        [Alias("GroupId")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceParameterSetName, ValueFromPipeline = true)]
        public Workspace Workspace { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        #endregion

        #region Constructors
        public AddPowerBIDataset() : base() { }

        public AddPowerBIDataset(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Organization)
            {
                throw new NotImplementedException($"{CmdletVerb}-{CmdletName} is only supported when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Individual)} is specified");
            }
        }

        public override void ExecuteCmdlet()
        {
            if(this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            object result;
            using (var client = this.CreateClient())
            {
                if(this.WorkspaceId != default)
                {
                    result = client.Datasets.AddDataset(this.Dataset, this.WorkspaceId);
                }
                else
                {
                    result = client.Datasets.AddDataset(this.Dataset);
                }
            }
            
            this.Logger.WriteObject(result, true);
        }
    }
}
