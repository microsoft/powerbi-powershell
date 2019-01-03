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
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [Alias("Get-PowerBIGroup")]
    [OutputType(typeof(IEnumerable<Workspace>))]
    public class GetPowerBIWorkspace : GetCmdlet, IUserScope, IUserFilter, IUserId, IUserFirstSkip
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";

        // Since internally, users are null rather than an empty list on workspaces v1 (groups), we don't need to filter on type for the time being
        private const string OrphanedFilterString = "(not users/any()) or (not users/any(u: u/groupUserAccessRight eq Microsoft.PowerBI.ServiceContracts.Api.GroupUserAccessRight'Admin'))";

        private string DeletedFilterString = string.Format("state eq '{0}'", WorkspaceState.Deleted);

        public GetPowerBIWorkspace() : base() { }

        public GetPowerBIWorkspace(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = AllParameterSetName)]
        public string Filter { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = AllParameterSetName)]
        public string User { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = AllParameterSetName)]
        public SwitchParameter Deleted { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = AllParameterSetName)]
        public SwitchParameter Orphaned { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AllParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = NameParameterSetName)]
        public new SwitchParameter All { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; } = 5000;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.First > 5000)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.First)} cannot be greater than 5000.");
            }

            if (this.All.IsPresent && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.All)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }

            if (!string.IsNullOrEmpty(this.User) && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.User)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.Deleted.IsPresent && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                // You can't view deleted workspaces when scope is Individual
                return;
            }

            if (this.Orphaned.IsPresent && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                // You can't have orphaned workspaces when scope is Individual as orphaned workspaces are unassigned
                return;
            }

            if (this.Deleted.IsPresent)
            {
                this.Filter = string.IsNullOrEmpty(this.Filter) ? DeletedFilterString : $"({this.Filter}) and ({DeletedFilterString})";
            }

            if (this.Orphaned.IsPresent)
            {
                this.Filter = string.IsNullOrEmpty(this.Filter) ? OrphanedFilterString : $"({this.Filter}) and ({OrphanedFilterString})";
            }

            if (this.ParameterSet.Equals(IdParameterSetName))
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.ParameterSet.Equals(NameParameterSetName))
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            if (this.All.IsPresent && this.Scope == PowerBIUserScope.Organization)
            {
                this.ExecuteCmdletWithAll();
                return;
            }

            if (!string.IsNullOrEmpty(this.User) && this.Scope == PowerBIUserScope.Organization)
            {
                var userFilter = $"users/any(u: tolower(u/emailAddress) eq '{this.User.ToLower()}')";
                this.Filter = string.IsNullOrEmpty(this.Filter) ? userFilter : $"({this.Filter}) and ({userFilter})";
            }

            using (var client = this.CreateClient())
            {
                var workspaces = this.Scope == PowerBIUserScope.Individual ?
                    client.Workspaces.GetWorkspaces(filter: this.Filter, top: this.First, skip: this.Skip) :
                    client.Workspaces.GetWorkspacesAsAdmin(expand: "users", filter: this.Filter, top: this.First, skip: this.Skip);
                this.Logger.WriteObject(workspaces, true);
            }
        }

        private void ExecuteCmdletWithAll()
        {
            using (var client = this.CreateClient())
            {
                Func<int, int, IEnumerable<Workspace>> GetWorkspaces =
                    (top, skip) => client.Workspaces.GetWorkspacesAsAdmin(expand: "users", filter: this.Filter, top: top, skip: skip);

                var allWorkspaces = this.ExecuteCmdletWithAll(GetWorkspaces);

                if (!string.IsNullOrEmpty(this.User))
                {
                    allWorkspaces = allWorkspaces
                        .Where(w => w.Users.Any(u => u.UserPrincipalName != null && u.UserPrincipalName.Equals(this.User, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                this.Logger.WriteObject(allWorkspaces, true);
            }
        }
    }
}