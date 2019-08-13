/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public interface IWorkspacesClient
    {
        object AddWorkspaceUser(Guid workspaceId, WorkspaceUser userAccessRight);

        object AddWorkspaceUserAsAdmin(Guid workspaceId, WorkspaceUser userAccessRight);

        IEnumerable<Workspace> GetWorkspaces(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Workspace> GetWorkspacesAsAdmin(string expand = default, string filter = default, int? top = default, int? skip = default);

        WorkspaceLastMigrationStatus GetWorkspaceLastMigrationStatus(Guid workspaceId);

        object UpdateWorkspaceAsAdmin(Guid workspaceId, Workspace updatedProperties);

        object RestoreDeletedWorkspaceAsAdmin(Guid workspaceId, WorkspaceRestoreRequest restoreRequest);

        object RemoveWorkspaceUser(Guid workspaceId, string userPrincipalName);

        object RemoveWorkspaceUserAsAdmin(Guid workspaceId, string userPrincipalName);

        object NewWorkspaceAsUser(string workspaceName);

        void MigrateWorkspaceCapacity(Guid workspaceId, Guid capacityId);
    }
}
