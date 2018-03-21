/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
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
    public class SetPowerBIWorkspace : PowerBICmdlet, IUserScope
    {
        public const string CmdletName = "PowerBIWorkspace";
        public const string CmdletVerb = VerbsCommon.Set;

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = true)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public string Description { get; set; }

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
            
            if (this.Scope.Equals(PowerBIUserScope.Individual))
            {
                throw new NotImplementedException();
            }

            var updatedGroup = new Group
            {
                Name = this.Name,
                Description = this.Description
            };

            var response = client.Groups.UpdateGroupAsAdmin(this.Id.ToString(), updatedGroup);
            this.Logger.WriteObject(response);
        }
    }
}
