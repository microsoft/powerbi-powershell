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
    public class GetPowerBIWorkspace : PowerBIGetCmdlet, IUserScope, IUserFilter, IUserId, IUserFirstSkip
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string TypeParameterSetName = "Type";
        private const string ListParameterSetName = "List";

        // Since internally, users are null rather than an empty list on workspaces v1 (groups), we don't need to filter on type for the time being
        private string OrphanedFilterString = $"(state ne '{WorkspaceState.Deleted}') and ((not users/any()) or (not users/any(u: u/groupUserAccessRight eq Microsoft.PowerBI.ServiceContracts.Api.GroupUserAccessRight'Admin')))";

        private string DeletedFilterString = $"state eq '{WorkspaceState.Deleted}'";

        public GetPowerBIWorkspace() : base() { }

        public GetPowerBIWorkspace(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = AllParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = NameParameterSetName)]
        public string Type { get; set; }

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

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = AllParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = IdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = NameParameterSetName)]
        [Alias("Expand")]
        public ArtifactType[] Include { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.First > 5000)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.First)} cannot be greater than 5000.");
            }

            if (!string.IsNullOrEmpty(this.User) && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.User)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }

            if (this.Include != null)
            {
                if (this.Include.Any() && this.Scope.Equals(PowerBIUserScope.Individual))
                {
                    this.Logger.ThrowTerminatingError($"{nameof(this.Include)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
                }

                if (this.Include.Contains(ArtifactType.All) && this.Include.Count() > 1)
                {
                    this.Logger.ThrowTerminatingError($"Parameter {nameof(this.Include)} is invalid. Enum {nameof(ArtifactType.All)} cannot be combined with other {nameof(ArtifactType)}");
                }
            }

            if (this.Deleted.IsPresent && this.Orphaned.IsPresent)
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Deleted)} & {nameof(this.Orphaned)} cannot be used together.");
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

            if (this.Include != null && this.Include.Any() && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                // You can't use Include/Expand when scope is Individual
                return;
            }

            if (this.All.IsPresent)
            {
                this.ExecuteCmdletWithAll();
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
                this.Filter = $"tolower(id) eq '{this.Id}'";
            }

            if (this.ParameterSet.Equals(NameParameterSetName))
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            if (!string.IsNullOrEmpty(this.Type))
            {
                var typeFilter = $"type eq '{this.Type}'";
                this.Filter = string.IsNullOrEmpty(this.Filter) ? typeFilter : $"({this.Filter}) and ({typeFilter})";
            }

            if (!string.IsNullOrEmpty(this.User) && this.Scope == PowerBIUserScope.Organization)
            {
                var userFilter = $"users/any(u: tolower(u/emailAddress) eq '{this.User.ToLower()}')";
                this.Filter = string.IsNullOrEmpty(this.Filter) ? userFilter : $"({this.Filter}) and ({userFilter})";
            }

            this.GetWorkspaces();
        }

        private void GetWorkspaces()
        {
            using (var client = this.CreateClient())
            {
                bool defaultingFirst = false;
                if (this.First == default)
                {
                    this.First = 100;
                    defaultingFirst = true;
                }

                var workspaces = this.Scope == PowerBIUserScope.Individual ?
                    client.Workspaces.GetWorkspaces(filter: this.Filter, top: this.First, skip: this.Skip) :
                    client.Workspaces.GetWorkspacesAsAdmin(expand: this.GetExpandClauseForWorkspace(), filter: this.Filter, top: this.First, skip: this.Skip);

                if (defaultingFirst && workspaces.Count() == 100)
                {
                    this.Logger.WriteWarning("Defaulted to show top 100 workspaces. Use -First & -Skip or -All to retrieve more results.");
                }

                this.Logger.WriteObject(workspaces, true);
            }
        }

        private void ExecuteCmdletWithAll()
        {
            using (var client = this.CreateClient())
            {
                if (this.Scope == PowerBIUserScope.Individual)
                {
                    var allWorkspacesOfUser = this.ExecuteCmdletWithAll((top, skip) => client.Workspaces.GetWorkspaces(filter: this.Filter, top: top, skip: skip));
                    this.Logger.WriteObject(allWorkspacesOfUser, true);
                    return;
                }

                var allWorkspaces = this.ExecuteCmdletWithAll((top, skip) => client.Workspaces.GetWorkspacesAsAdmin(expand: this.GetExpandClauseForWorkspace(), filter: this.Filter, top: top, skip: skip));

                var filteredWorkspaces = new List<Workspace>();

                if (!string.IsNullOrEmpty(this.User))
                {
                    allWorkspaces = allWorkspaces
                        .Where(w => w.Users != null && w.Users.Any(u => u.UserPrincipalName != null && u.UserPrincipalName.Equals(this.User, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                if (this.Deleted.IsPresent)
                {
                    filteredWorkspaces.AddRange(allWorkspaces.Where(w => w.State.Equals(WorkspaceState.Deleted)));
                }
                else if (this.Orphaned.IsPresent)
                {
                    filteredWorkspaces.AddRange(allWorkspaces.Where(w => w.IsOrphaned));
                }
                else
                {
                    this.Logger.WriteObject(allWorkspaces, true);
                    return;
                }

                this.Logger.WriteObject(filteredWorkspaces, true);
            }
        }

        private string GetExpandClauseForWorkspace()
        {
            this.Include = this.Include ?? new ArtifactType[0];
            if (this.Include.Contains(ArtifactType.All))
            {
                this.Include = new ArtifactType[]
                {
                    ArtifactType.Reports,
                    ArtifactType.Dashboards,
                    ArtifactType.Datasets,
                    ArtifactType.Dataflows,
                    ArtifactType.Workbooks,
                };
            }

            var includeArtifactTypes = string.Join(separator: ",", this.Include).ToLowerInvariant();
            var includeArtifactTypesWithUsers = string.IsNullOrEmpty(includeArtifactTypes) ?
                "users" : string.Concat("users", ",", includeArtifactTypes);
            return includeArtifactTypesWithUsers;
        }
    }
}