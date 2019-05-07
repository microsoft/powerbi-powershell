/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public interface IWorkspacesClient
    {
        void AddWorkspaceUser(Guid workspaceId, WorkspaceUser userAccessRight);

        void AddWorkspaceUserAsAdmin(Guid workspaceId, WorkspaceUser userAccessRight);

        IEnumerable<Workspace> GetWorkspaces(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Workspace> GetWorkspacesAsAdmin(int top, string expand = default, string filter = default, int? skip = default);

        void UpdateWorkspaceAsAdmin(Guid workspaceId, Workspace updatedProperties);

        void RestoreDeletedWorkspaceAsAdmin(Guid workspaceId, WorkspaceRestoreRequest restoreRequest);

        void RemoveWorkspaceUser(Guid workspaceId, string userPrincipalName);

        void RemoveWorkspaceUserAsAdmin(Guid workspaceId, string userPrincipalName);

        object NewWorkspaceAsUser(string workspaceName);

    }
}
