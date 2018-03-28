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
using Microsoft.Rest;

namespace Microsoft.PowerBI.Commands.Workspaces
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Group>))]
    public class GetPowerBIWorkspace : PowerBICmdlet, IUserScope, IUserFilter
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        private const string OrphanedFilterString = "(not users/any()) or (not users/any(u: u/groupUserAccessRight eq Microsoft.PowerBI.ServiceContracts.Api.GroupUserAccessRight'Admin'))";

        #region Parameters

        [Parameter(Mandatory = false)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false)]
        public string Filter { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Orphaned { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false)]
        public int? Skip { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if(this.Orphaned.IsPresent && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"Orphaned is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }

            if(!string.IsNullOrEmpty(this.Filter) && this.Scope.Equals(PowerBIUserScope.Individual))
            {
                this.Logger.ThrowTerminatingError($"Filter is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }
        }

        protected override void ExecuteCmdlet()
        {
            PowerBIClient client = null;
            var token = this.Authenticator.Authenticate(this.Profile, this.Logger, this.Settings);
            if (Uri.TryCreate(this.Profile.Environment.GlobalServiceEndpoint, UriKind.Absolute, out Uri baseUri))
            {
                client = new PowerBIClient(baseUri, new TokenCredentials(token.AccessToken));
            }
            else
            {
                client = new PowerBIClient(new TokenCredentials(token.AccessToken));
            }

            if(this.Orphaned.IsPresent)
            {
                this.Filter = string.IsNullOrEmpty(this.Filter) ? OrphanedFilterString : $"({this.Filter}) and ({OrphanedFilterString})";
            }

            if(this.Id != default && this.Scope == PowerBIUserScope.Organization)
            {
                var idFilter = $"id eq '{this.Id}'";
                this.Filter = string.IsNullOrEmpty(this.Filter) ? idFilter : $"({idFilter}) and ({this.Filter})";
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

                if (this.Skip.GetValueOrDefault() > 0)
                {
                    workspaces = workspaces.Skip(this.Skip.Value).ToList();
                }

                if (this.First.GetValueOrDefault() > 0)
                {
                    workspaces = workspaces.Take(this.First.Value).ToList();
                }
            }

            this.Logger.WriteObject(workspaces, true);
        }
    }
}