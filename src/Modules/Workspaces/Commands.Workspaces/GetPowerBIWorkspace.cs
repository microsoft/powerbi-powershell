/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Client;
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [Alias("Get-PowerBIGroup")]
    [OutputType(typeof(IEnumerable<Group>))]
    public class GetPowerBIWorkspace : PowerBIClientCmdlet, IUserScope, IUserFilter, IUserId
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";

        private const string OrphanedFilterString = "(not users/any()) or (not users/any(u: u/groupUserAccessRight eq Microsoft.PowerBI.ServiceContracts.Api.GroupUserAccessRight'Admin'))";

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("GroupId", "WorkspaceId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string Filter { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string User { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public SwitchParameter Orphaned { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if(!string.IsNullOrEmpty(this.Filter) && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Filter)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }

            if (!string.IsNullOrEmpty(this.User) && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.User)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }
        }

        protected override void ExecuteCmdlet()
        {
            if (this.Orphaned.IsPresent && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                // You can't have orphaned workspaces when scope is individual as orphaned workspaces are unassigned
                return;
            }

            IPowerBIClient client = this.CreateClient();

            if(this.Orphaned.IsPresent)
            {
                this.Filter = string.IsNullOrEmpty(this.Filter) ? OrphanedFilterString : $"({this.Filter}) and ({OrphanedFilterString})";
            }

            if(this.ParameterSetName == IdParameterSetName && this.Scope == PowerBIUserScope.Organization)
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.ParameterSetName == NameParameterSetName && this.Scope == PowerBIUserScope.Organization)
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            if(!string.IsNullOrEmpty(this.User) && this.Scope == PowerBIUserScope.Organization)
            {
                var userFilter = $"users/any(u: tolower(u/emailAddress) eq '{this.User.ToLower()}')";
                this.Filter = string.IsNullOrEmpty(this.Filter) ? userFilter : $"({this.Filter}) and ({userFilter})";
            }

            var workspacesResult = this.Scope == PowerBIUserScope.Individual ? 
                client.Groups.GetGroups() : 
                client.Groups.GetGroupsAsAdmin(expand: "users", filter: this.Filter, top: this.First, skip: this.Skip);
            var workspaces = workspacesResult.Value;
            if (this.Scope == PowerBIUserScope.Individual)
            {
                // non-Admin API doesn't support filter, top, skip; processing after getting result
                if (this.Id != default)
                {
                    workspaces = workspaces.Where(w => this.Id == new Guid(w.Id)).ToList();
                }

                if(!string.IsNullOrEmpty(this.Name))
                {
                    workspaces = workspaces.Where(w => this.Name.Equals(w.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (this.Skip.GetValueOrDefault() > 0)
                {
                    workspaces = workspaces.Skip(this.Skip.Value).ToList();
                }

                if (this.First.GetValueOrDefault() > 0)
                {
                    workspaces = workspaces.Take(this.First.Value).ToList();
                }
            }

            if(this.Scope == PowerBIUserScope.Organization)
            {
                this.Logger.WriteWarning($"Only preview workspaces are returned when -{nameof(this.Scope)} {nameof(PowerBIUserScope.Organization)} is specified");
            }

            this.Logger.WriteObject(workspaces, true);
        }
    }
}