/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class ExportPowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsData.Export;
        public const string CmdletName = "PowerBIReport";

        #region Parameters
        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }

        [Alias("ReportId")]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true)]
        public string OutFile { get; set; }
        #endregion

        #region Constructors
        public ExportPowerBIReport() : base() { }

        public ExportPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(this.OutFile))
            {
                this.OutFile = this.ResolveFilePath(this.OutFile, false);
                if (File.Exists(this.OutFile))
                {
                    this.Logger.ThrowTerminatingError(new NotSupportedException($"OutFile '{this.OutFile}' already exists, specify a new file path"), ErrorCategory.InvalidArgument);
                }
            }

            using (var client = this.CreateClient())
            {
                using (var reportStream = client.Reports.ExportReport(this.Id, this.WorkspaceId))
                {
                    if (reportStream != null)
                    {
                        using (var fileStream = new FileStream(this.OutFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                        {
                            reportStream.CopyTo(fileStream);
                        }

                        this.Logger.WriteVerbose($"OutFile '{this.OutFile}' created");
                    }
                    else
                    {
                        var workspaceMessage = this.WorkspaceId != default ? $" with workspace ID {this.WorkspaceId}" : string.Empty;
                        this.Logger.WriteError($"Failed to export report with ID {this.Id}{workspaceMessage}");
                    }
                }
            }
        }
    }
}
