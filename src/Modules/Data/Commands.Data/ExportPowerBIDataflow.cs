/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class ExportPowerBIDataflow : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsData.Export;
        public const string CmdletName = "PowerBIDataflow";

        #region Parameters
        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }

        [Alias("DataflowId")]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true)]
        public string OutFile { get; set; }
        #endregion

        #region Constructors
        public ExportPowerBIDataflow() : base() { }

        public ExportPowerBIDataflow(IPowerBIClientCmdletInitFactory init) : base(init) { }
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

                using (var dataflowStream = this.Scope == PowerBIUserScope.Organization ?
                    client.Dataflows.ExportDataflowAsAdmin(this.Id) :
                    client.Dataflows.GetDataflow(this.WorkspaceId, this.Id))
                {
                    if (dataflowStream != null)
                    {
                        using (var fileStream = new FileStream(this.OutFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                        {
                            dataflowStream.CopyTo(fileStream);
                        }

                        this.Logger.WriteVerbose($"OutFile '{this.OutFile}' created");
                    }
                    else
                    {
                        var workspaceMessage = this.WorkspaceId != default ? $" with workspace ID {this.WorkspaceId}" : string.Empty;
                        this.Logger.WriteError($"Failed to export dataflow with ID {this.Id}{workspaceMessage}");
                    }
                }
            }
        }
    }
}
