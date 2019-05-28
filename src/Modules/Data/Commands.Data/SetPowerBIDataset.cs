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
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DatasetIdParameterSetName)]
    public class SetPowerBIDataset : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.Set;
        public const string CmdletName = "PowerBIDataset";

        #region ParameterSets
        private const string DatasetIdParameterSetName = "DatasetId";
        private const string DatasetNameParameterSetName = "DatasetName";
        private const string ObjectIdParameterSetName = "ObjectAndId";
        private const string ObjectNameParameterSetName = "ObjectAndName";
        #endregion

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DatasetNameParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = DatasetNameParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = ObjectNameParameterSetName)]
        [ValidateSet("Abf", "PremiumFiles")]
        public DatasetStorageMode TargetStorageMode { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetNameParameterSetName)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = false, ParameterSetName = ObjectIdParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = false, ParameterSetName = ObjectNameParameterSetName, ValueFromPipeline = true)]
        public Workspace Workspace { get; set; }

        #endregion

        #region Constructors
        public SetPowerBIDataset() : base() { }

        public SetPowerBIDataset(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            using (var client = this.CreateClient())
            {
                client.Datasets.PatchDataset(
                    this.Id,
                    new PatchDatasetRequest
                    {
                        TargetStorageMode = this.TargetStorageMode,
                    },
                    this.WorkspaceId);
            }
        }
    }
}
