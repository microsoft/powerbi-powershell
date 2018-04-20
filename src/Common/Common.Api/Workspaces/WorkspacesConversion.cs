/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Linq;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public static class WorkspacesConversion
    {
        public static Workspace ToWorkspace(Group group)
        {
            return new Workspace
            {
                Id = new Guid(group.Id),
                Name = group.Name,
                IsReadOnly = group.IsReadOnly,
                IsOnDedicatedCapacity = group.IsOnDedicatedCapacity,
                CapacityId = group.CapacityId,
                Description = group.Description,
                Type = group.Type,
                State = group.State,
                Users = group.Users?.Select(x => ToWorkspaceUser(x)),
            };
        }

        public static Group ToGroup(Workspace workspace)
        {
            return new Group
            {
                Id = workspace.Id.ToString(),
                Name = workspace.Name,
                IsReadOnly = workspace.IsReadOnly,
                IsOnDedicatedCapacity = workspace.IsOnDedicatedCapacity,
                CapacityId = workspace.CapacityId,
                Description = workspace.Description,
                Type = workspace.Type,
                State = workspace.State,
                Users = workspace.Users?.Select(x => ToGroupUserAccessRight(x)).ToList(),
            };
        }

        public static WorkspaceUser ToWorkspaceUser(GroupUserAccessRight groupUserAccessRight)
        {
            return new WorkspaceUser { AccessRight = groupUserAccessRight.GroupUserAccessRightProperty, UserPrincipalName = groupUserAccessRight.EmailAddress };
        }

        public static GroupUserAccessRight ToGroupUserAccessRight(WorkspaceUser workspaceUser)
        {
            return new GroupUserAccessRight { GroupUserAccessRightProperty = workspaceUser.AccessRight, EmailAddress = workspaceUser.UserPrincipalName };
        }

        public static GroupRestoreRequest ToGroupRestoreRequest(WorkspaceRestoreRequest workspaceRestoreRequest)
        {
            return new GroupRestoreRequest { Name = workspaceRestoreRequest.RestoredName, EmailAddress = workspaceRestoreRequest.UserPrincipalName };
        }
    }
}
