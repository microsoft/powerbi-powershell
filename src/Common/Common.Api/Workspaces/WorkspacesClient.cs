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
            return this.Client.Groups.AddGroupUser(workspaceId.ToString(), userAccessRight);
        }

        public object AddWorkspaceUserAsAdmin(Guid workspaceId, WorkspaceUser userAccessRight)
        {
            return this.Client.Groups.AddUserAsAdmin(workspaceId.ToString(), userAccessRight);
        }

        public IEnumerable<Workspace> GetWorkspaces(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Groups.GetGroups(filter, top, skip).Value.Select(x => (Workspace)x);
        }

        public IEnumerable<Workspace> GetWorkspacesAsAdmin(string expand = null, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Groups.GetGroupsAsAdmin(expand, filter, top, skip).Value.Select(x => (Workspace)x);
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
            return this.Client.Groups.RestoreDeletedGroupAsAdmin(workspaceId.ToString(), restoreRequest);
        }

        public object UpdateWorkspaceAsAdmin(Guid workspaceId, Workspace updatedProperties)
        {
            return this.Client.Groups.UpdateGroupAsAdmin(workspaceId.ToString(), updatedProperties);
        }

        public object NewWorkspaceAsUser(string workspaceName)
        {
            return (Workspace)this.Client.Groups.CreateGroup(new PowerBI.Api.V2.Models.GroupCreationRequest(name: workspaceName));
        }

        public object MigrateWorkspaceCapacity(Guid workspaceId, Guid capacityId)
        {
            if (capacityId != Guid.Empty)
            {
                var assignment = new PowerBI.Api.V2.Models.CapacityMigrationAssignment
                {
                    WorkspacesToAssign = new string[] { workspaceId.ToString() },
                    TargetCapacityObjectId = capacityId.ToString(),
                };

                var request = new PowerBI.Api.V2.Models.AssignWorkspacesToCapacityRequest
                {
                    CapacityMigrationAssignments = { assignment },
                };

                return this.Client.Capacities.MoveWorkspacesToPremiumCapacity(request);
            }
            else
            {
                var request = new PowerBI.Api.V2.Models.UnassignWorkspacesCapacityRequest
                {
                    WorkspacesToUnassign = new string[] { workspaceId.ToString() },
                };

                return this.Client.Capacities.MoveWorkspacesToSharedCapacity(request);
            }
        }
    }
}
