/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
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
    public class AddPowerBIWorkspaceMember : PowerBICmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspaceMember";
        public const string CmdletVerb = VerbsCommon.Add;

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true)]
        public Guid GroupId { get; set; }

        [Parameter(Mandatory = true)]
        public GroupUserAccessRight UserDetails { get; set; }

        #endregion

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

            var result = this.Scope.Equals(PowerBIUserScope.Individual) ? client.Groups.AddGroupUser(GroupId.ToString(), UserDetails) : client.Groups.AddGroupUser(GroupId.ToString(), UserDetails);
            this.Logger.WriteObject(result, true);
        }
    }
}