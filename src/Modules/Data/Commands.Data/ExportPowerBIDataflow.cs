/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.IO;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Dataflows;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = IdParameterSetName)]
    public class ExportPowerBIDataflow : PowerBIClientCmdlet, IUserId, IUserScope
    {
        public const string CmdletVerb = VerbsData.Export;
        public const string CmdletName = "PowerBIDataflow";

        #region ParameterSets
        private const string IdParameterSetName = "Id";
        private const string DataflowParameterSetName = "Dataflow";
        #endregion

        #region Parameters
        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }

        [Alias("DataflowId")]
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DataflowParameterSetName)]
        public Dataflow Dataflow { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true)]
        public string OutFile { get; set; }
        #endregion

        #region Constructors
        public ExportPowerBIDataflow() : base() { }

        public ExportPowerBIDataflow(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Individual && this.WorkspaceId == null)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.WorkspaceId)} must be applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Individual)}");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(this.OutFile))
            {
                this.OutFile = this.ResolveFilePath(this.OutFile, false);
                if (File.Exists(this.OutFile))
                {
                    this.Logger.ThrowTerminatingError(new IOException($"OutFile '{this.OutFile}' already exists, specify a new file path"), ErrorCategory.ResourceExists);
                }
            }

            if (this.Dataflow != null)
            {
                this.Id = this.Dataflow.Id;

                this.Logger.WriteDebug($"Using {nameof(this.Dataflow)} object to get {nameof(this.Id)} parameter. Value: {this.Id}");
            }

            using (var client = this.CreateClient())
            {
                using (var dataflowStream = this.Scope == PowerBIUserScope.Organization ?
                    client.Dataflows.ExportDataflowAsAdmin(this.Id) :
                    client.Dataflows.GetDataflow(this.WorkspaceId, this.Id))
                {
                    if (dataflowStream != null)
                    {
                        try
                        {
                            using (var fileStream = new FileStream(this.OutFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                            {
                                dataflowStream.CopyTo(fileStream);
                            }

                            this.Logger.WriteVerbose($"OutFile '{this.OutFile}' created");
                        }
                        catch (Exception ex)
                        {
                            this.Logger.WriteError($"Failed to write dataflow with ID {this.Id} to file {this.OutFile}. Message: {ex.Message}");
                        }
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
