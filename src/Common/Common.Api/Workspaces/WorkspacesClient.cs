/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class WorkspacesClient : PowerBIEntityClient, IWorkspacesClient
    {
        public WorkspacesClient(IPowerBIClient client) : base(client)
        {
        }

        public object AddWorkspaceUser(Guid workspaceId, WorkspaceUser userAccessRight)
        {
            return this.Client.Groups.AddGroupUser(workspaceId.ToString(), WorkspacesConversion.ToGroupUserAccessRight(userAccessRight));
        }

        public object AddWorkspaceUserAsAdmin(Guid workspaceId, WorkspaceUser userAccessRight)
        {
            return this.Client.Groups.AddUserAsAdmin(workspaceId.ToString(), WorkspacesConversion.ToGroupUserAccessRight(userAccessRight));
        }

        public IEnumerable<Workspace> GetWorkspaces(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Groups.GetGroups(filter, top, skip).Value.Select(x => WorkspacesConversion.ToWorkspace(x));
        }

        public IEnumerable<Workspace> GetWorkspacesAsAdmin(string expand = null, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Groups.GetGroupsAsAdmin(expand, filter, top, skip).Value.Select(x => WorkspacesConversion.ToWorkspace(x));
        }

        public object RemoveWorkspaceUser(Guid workspaceId, string userPrincipalName)
        {
            return this.Client.Groups.DeleteUserInGroup(workspaceId.ToString(), userPrincipalName);
        }

        public object RemoveWorkspaceUserAsAdmin(Guid workspaceId, string userPrincipalName)
        {
            return this.Client.Groups.DeleteUserAsAdmin(workspaceId.ToString(), userPrincipalName);
        }

        public object RestoreDeletedWorkspaceAsAdmin(Guid workspaceId, WorkspaceRestoreRequest restoreRequest)
        {
            return this.Client.Groups.RestoreDeletedGroupAsAdmin(workspaceId.ToString(), WorkspacesConversion.ToGroupRestoreRequest(restoreRequest));
        }

        public object UpdateWorkspaceAsAdmin(Guid workspaceId, Workspace updatedProperties)
        {
            return this.Client.Groups.UpdateGroupAsAdmin(workspaceId.ToString(), WorkspacesConversion.ToGroup(updatedProperties));
        }
    }
}
