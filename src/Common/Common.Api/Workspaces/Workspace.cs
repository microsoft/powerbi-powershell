/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class Workspace
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool? IsReadOnly { get; set; }

        public bool? IsOnDedicatedCapacity { get; set; }

        public Guid? CapacityId { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string State { get; set; }

        public bool IsOrphaned
        {
            get
            {
                if (this.State == null || this.State.Equals(WorkspaceState.Deleted) || this.Type.Equals(WorkspaceType.Group) || this.Type.Equals(WorkspaceType.PersonalGroup))
                {
                    return false;
                }

                return (this.Users == null) || (!this.Users.Any(u => u.AccessRight.Equals(WorkspaceUserAccessRight.Admin)));
            }
        }

        public IEnumerable<WorkspaceUser> Users { get; set; }

        public static implicit operator Workspace(Group group)
        {
            return new Workspace
            {
                Id = group.Id,
                Name = group.Name,
                IsReadOnly = group.IsReadOnly,
                IsOnDedicatedCapacity = group.IsOnDedicatedCapacity,
                CapacityId = group.CapacityId,
                Description = group.Description,
                Type = group.Type,
                State = group.State,
                Users = group.Users?.Select(x => (WorkspaceUser)x)
            };
        }

        public static implicit operator Group(Workspace workspace)
        {
            return new Group
            {
                Id = workspace.Id,
                Name = workspace.Name,
                IsReadOnly = workspace.IsReadOnly,
                IsOnDedicatedCapacity = workspace.IsOnDedicatedCapacity,
                CapacityId = workspace.CapacityId,
                Description = workspace.Description,
                Type = workspace.Type,
                State = workspace.State,
                Users = workspace.Users?.Select(x => (Microsoft.PowerBI.Api.V2.Models.GroupUser)x).ToList()
            };
        }
    }
}
