using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Tile>))]
    public class GetPowerBITile : PowerBIClientCmdlet, IUserScope, IUserId, IUserFirstSkip
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBITile";

        private const string IdParameterSetName = "Id";
        private const string ListParameterSetName = "List";

        #region Parameters
        [Parameter(Mandatory = true)]
        public Guid DashboardId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("ImportId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false)]
        public Guid WorkspaceId { get; set; }
        #endregion

        #region Constructors
        public GetPowerBITile() : base() { }

        public GetPowerBITile(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (this.Scope == PowerBIUserScope.Organization && this.WorkspaceId != default)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.WorkspaceId)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Individual)}", ErrorCategory.InvalidArgument);
            }
        }

        public override void ExecuteCmdlet()
        {
            IEnumerable<Tile> tiles = null;
            using (var client = this.CreateClient())
            {
                if (this.WorkspaceId != default)
                {
                    tiles = client.Reports.GetTilesForWorkspace(workspaceId: this.WorkspaceId, dashboardId: this.DashboardId);
                }
                else
                {
                    tiles = this.Scope == PowerBIUserScope.Individual ?
                        client.Reports.GetTiles(this.DashboardId) :
                        client.Reports.GetTilesAsAdmin(this.DashboardId);
                }
            }

            // TODO filter result

            this.Logger.WriteObject(tiles, true);
        }
    }
}
