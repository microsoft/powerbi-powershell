/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = MyWorkspaceParameterSetName)]
    [OutputType(typeof(Tile))]
    public class CopyPowerBITile : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.Copy;
        public const string CmdletName = "PowerBITile";

        #region Constructors
        public CopyPowerBITile() : base() { }

        public CopyPowerBITile(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        #region ParameterSets
        private const string MyWorkspaceParameterSetName = "MyWorkspace";
        private const string WorkspaceIdParameterSetName = "WorkspaceId";
        private const string WorkspaceObjectParameterSetName = "WorkspaceObject";
        #endregion

        #region Parameters
        //The id of the workspace where the source dashboard is located. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'.
        [Alias("GroupId")]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceIdParameterSetName)]
        public Guid WorkspaceId  { get; set; }

         //The workspace where the source dashboard is located.
        [Alias("Group")]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceObjectParameterSetName)]
        public Workspace Workspace { get; set; }

        //The id of the dashboard where source tile is located.
        [Alias("DashboardKey")]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceObjectParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = MyWorkspaceParameterSetName)]
        public string DashboardId { get; set; }
        
        //The id of the tile that should be copied
        [Alias("TileKey")]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceObjectParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = MyWorkspaceParameterSetName)]
        public string TileId { get; set; }

        //The id of the dashboard where tile copy should be added.
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceObjectParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = WorkspaceIdParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = MyWorkspaceParameterSetName)]
        public string TargetDashboardId { get; set; }

        //Optional parameter for specifying the target workspace id. Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'. 
        //Empty string indicates tile will be copied within the same workspace.
        [Alias("TargetGroupId")]
        [Parameter(Mandatory = false)]
        public string TargetWorkspaceId { get; set; }
        
        //Optional parameter when cloning a tile linked to a report, to rebind the new tile to a different report.
        [Parameter(Mandatory = false)]
        public string TargetReportId { get; set; }

        //Optional parameter when cloning a tile linked to a dataset, to rebind the new tile to a different dataset.
        [Alias("TargetModelId")]
        [Parameter(Mandatory = false)]
        public string TargetDatasetId { get; set; }

        //Optional parameter for specifying the action in case of position conflict. The default is 'Tail'.
        [Parameter(Mandatory = false)]
        public string PositionConflictAction { get; set; }
        #endregion
        public override void ExecuteCmdlet()
        {
            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            using (var client = this.CreateClient())
            {
                var result = client.Reports.CopyTile(
                    this.WorkspaceId,
                    this.DashboardId,
                    this.TileId,
                    this.TargetDashboardId, 
                    this.TargetWorkspaceId, 
                    this.TargetReportId,
                    this.TargetDatasetId, 
                    this.PositionConflictAction);
                this.Logger.WriteObject(result, true);
            }
        }
    }
}
