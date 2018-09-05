/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DatasetParameterSetName)]
    [Alias("Remove-PowerBIRows")]
    [OutputType(typeof(Dataset))]
    public class RemovePowerBIRow : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.Remove;
        public const string CmdletName = "PowerBIRow";

        #region ParameterSets
        private const string DatasetIdParameterSetName = "DatasetId";
        private const string DatasetParameterSetName = "Dataset";
        #endregion

        #region Parameters
        [Parameter(Mandatory = true, ParameterSetName = DatasetIdParameterSetName)]
        public Guid DatasetId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = DatasetParameterSetName)]
        public Dataset Dataset { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = DatasetParameterSetName)]
        public string TableName { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName, ValueFromPipeline = true)]
        public Workspace Workspace { get; set; }

        #endregion

        #region Constructors
        public RemovePowerBIRow() : base() { }

        public RemovePowerBIRow(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            if(this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }
            
            if(this.Dataset != null)
            {
                this.DatasetId = this.Dataset.Id;
            }

            object result;
            using (var client = this.CreateClient())
            {
                if(this.WorkspaceId != default)
                {
                    result = client.Datasets.DeleteRows(this.DatasetId.ToString(), this.TableName, this.WorkspaceId);
                }
                else
                {
                    result = client.Datasets.DeleteRows(this.DatasetId.ToString(), this.TableName);
                }
            }
            
            this.Logger.WriteObject(result, true);
        }
    }
}
