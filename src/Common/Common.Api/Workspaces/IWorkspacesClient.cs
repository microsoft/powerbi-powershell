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
        object AddWorkspaceUser(Guid workspaceId, WorkspaceUser userAccessRight);

        object AddWorkspaceUserAsAdmin(Guid workspaceId, WorkspaceUser userAccessRight);

        IEnumerable<Workspace> GetWorkspaces(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Workspace> GetWorkspacesAsAdmin(string expand = default, string filter = default, int? top = default, int? skip = default);

        object UpdateWorkspaceAsAdmin(Guid workspaceId, Workspace updatedProperties);

        object RestoreDeletedWorkspaceAsAdmin(Guid workspaceId, WorkspaceRestoreRequest restoreRequest);

        object RemoveWorkspaceUser(Guid workspaceId, string userPrincipalName);

        object RemoveWorkspaceUserAsAdmin(Guid workspaceId, string userPrincipalName);
    }
}
