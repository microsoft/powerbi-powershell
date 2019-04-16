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
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = IdParameterSetName)]
    [OutputType(typeof(Report))]
    public class CopyPowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIReport";
        public const string CmdletVerb = VerbsCommon.Copy;

        #region Constructors
        public CopyPowerBIReport() : base() { }

        public CopyPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        #region ParameterSets
        private const string IdParameterSetName = "Id";
        private const string ObjectParameterSetName = "WorkspaceObject";
        private const string NameAndObjectParameterSetName = "NameAndWorkspaceObject";
        #endregion

        #region Parameters
        // Required. Unless a Report object is passed in, in which case the name of the original report can be used.
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = ObjectParameterSetName)]
        public string Name { get; set; }

        // Required. Unless a Report object is passed in.
        [Alias("ReportId")]
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        // Required. Unless a Report Id is passed in.
        [Parameter(Mandatory = true, ParameterSetName = ObjectParameterSetName)]
        public Report Report { get; set; }

        // Optional. If omitted, the report is copied from 'My Workspace'. 
        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public string WorkspaceId { get; set; }

        // Optional. If omitted, the report is copied from 'My Workspace'. 
        [Alias("Group")]
        [Parameter(Mandatory = false)]
        public Workspace Workspace { get; set; }

        // Optional. If omitted, the report is copied within the same workspace as the source report.
        // Empty Guid (00000000-0000-0000-0000-000000000000) indicates 'My Workspace'. 
        [Alias("TargetGroupId")]
        [Parameter(Mandatory = false)]
        public string TargetWorkspaceId { get; set; }

        // Optional. If omitted, the new report will be associated with the same dataset as the source report.
        [Alias("TargetModelId")]
        [Parameter(Mandatory = false)]
        public string TargetDatasetId { get; set; }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (this.Report != null)
            {
                this.Id = this.Report.Id;
                if (string.IsNullOrWhiteSpace(this.Name))
                {
                    this.Name = this.Report.Name;
                }
            }

            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id.ToString();
            }

            using (var client = this.CreateClient())
            {
                var result = client.Reports.CopyReport(
                    this.Name,
                    this.WorkspaceId,
                    this.Id.ToString(),
                    this.TargetWorkspaceId,
                    this.TargetDatasetId);
                this.Logger.WriteObject(result, true);
            }
        }

    }
}
