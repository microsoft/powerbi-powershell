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
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = NameOnlyParameterSetName)]
    [OutputType(typeof(Dashboard))]
    public class NewPowerBIDashboard : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.New;
        public const string CmdletName = "PowerBIDashboard";

        #region Constructors
        public NewPowerBIDashboard() : base() { }

        public NewPowerBIDashboard(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        #region ParameterSets
        private const string NameOnlyParameterSetName = "NameOnly";
        private const string NameAndIdsParameterSetName = "NameAndIds";
        private const string NameAndObjectParameterSetName = "NameAndObject";
        #endregion

        #region Parameters
        // The name of the new dashboard.
        [Parameter(Mandatory = true, ParameterSetName = NameOnlyParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = NameAndIdsParameterSetName)]
        [Parameter(Mandatory = true, ParameterSetName = NameAndObjectParameterSetName)]
        public string Name { get; set; }

        // Optional. If omitted, the dashboard is created in 'My Workspace'. 
        [Alias("GroupId")]
        [Parameter(Mandatory = true, ParameterSetName = NameAndIdsParameterSetName)]
        public Guid WorkspaceId { get; set; }

        // Optional. If omitted, the dashboard is created in 'My Workspace'. 
        [Alias("Group")]
        [Parameter(Mandatory = true, ParameterSetName = NameAndObjectParameterSetName)]
        public Workspace Workspace { get; set; }
        #endregion

        public override void ExecuteCmdlet()
        {
            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            using (var client = this.CreateClient())
            {
                var result = client.Reports.AddDashboard(this.Name, this.WorkspaceId);
                this.Logger.WriteObject(result, true);
            }
        }
    }
}
