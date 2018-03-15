/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

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
    public class GetPowerBIWorkspace : PowerBICmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Get;

        #region Parameters

        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        #endregion

        protected override void ExecuteCmdlet()
        {
            var token = this.Authenticator.Authenticate(this.Profile, this.Logger, this.Settings);
            var client = new PowerBIClient(new TokenCredentials(token.AccessToken));

            var workspaces = this.Scope.Equals(PowerBIUserScope.Individual) ? client.Groups.GetGroups() : client.Groups.GetGroupsAsAdmin();
            this.Logger.WriteObject(workspaces.Value, true);
        }
    }
}