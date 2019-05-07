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

        public void AddWorkspaceUser(Guid workspaceId, WorkspaceUser userAccessRight)
        {
            this.Client.Groups.AddGroupUser(workspaceId, userAccessRight);
        }

        public void AddWorkspaceUserAsAdmin(Guid workspaceId, WorkspaceUser userAccessRight)
        {
            this.Client.Groups.AddUserAsAdmin(workspaceId, userAccessRight);
        }

        public IEnumerable<Workspace> GetWorkspaces(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Groups.GetGroups(filter, top, skip).Value.Select(x => (Workspace)x);
        }

        public IEnumerable<Workspace> GetWorkspacesAsAdmin(int top, string expand = null, string filter = null, int? skip = null)
        {
            return this.Client.Groups.GetGroupsAsAdmin(top, expand, filter, skip).Value.Select(x => (Workspace)x);
        }

        public void RemoveWorkspaceUser(Guid workspaceId, string userPrincipalName)
        {
            this.Client.Groups.DeleteUserInGroup(workspaceId, userPrincipalName);
        }

        public void RemoveWorkspaceUserAsAdmin(Guid workspaceId, string userPrincipalName)
        {
            this.Client.Groups.DeleteUserAsAdmin(workspaceId, userPrincipalName);
        }

        public void RestoreDeletedWorkspaceAsAdmin(Guid workspaceId, WorkspaceRestoreRequest restoreRequest)
        {
            this.Client.Groups.RestoreDeletedGroupAsAdmin(workspaceId, restoreRequest);
        }

        public void UpdateWorkspaceAsAdmin(Guid workspaceId, Workspace updatedProperties)
        {
            this.Client.Groups.UpdateGroupAsAdmin(workspaceId, updatedProperties);
        }

        public object NewWorkspaceAsUser(string workspaceName)
        {
            return (Workspace)this.Client.Groups.CreateGroup(new PowerBI.Api.V2.Models.GroupCreationRequest(name: workspaceName));
        }
    }
}
