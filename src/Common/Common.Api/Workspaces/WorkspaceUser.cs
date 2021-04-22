/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class WorkspaceUser
    {
        public string AccessRight { get; set; }

        public string UserPrincipalName { get; set; }

        public string Identifier { get; set; }

        public WorkspaceUserPrincipalType? PrincipalType { get; set; }

        public static implicit operator WorkspaceUser(GroupUserAccessRight groupUserAccessRight)
        {
            if (!string.IsNullOrEmpty(groupUserAccessRight.PrincipalType) && Enum.TryParse(groupUserAccessRight.PrincipalType, out WorkspaceUserPrincipalType principalType))
            {
                // User principal type
                if (principalType == WorkspaceUserPrincipalType.User)
                {
                    return new WorkspaceUser { AccessRight = groupUserAccessRight.GroupUserAccessRightProperty, UserPrincipalName = groupUserAccessRight.Identifier, Identifier = groupUserAccessRight.Identifier, PrincipalType = principalType };
                }

                // Principal type is explicitly set.
                return new WorkspaceUser { AccessRight = groupUserAccessRight.GroupUserAccessRightProperty, Identifier = groupUserAccessRight.Identifier, PrincipalType = principalType };
            }
            else
            {
                // Principal type is not set, default to adding a user by their email address.
                return new WorkspaceUser { AccessRight = groupUserAccessRight.GroupUserAccessRightProperty, UserPrincipalName = groupUserAccessRight.EmailAddress };
            }
        }

        public static implicit operator GroupUserAccessRight(WorkspaceUser workspaceUser)
        {
            if (workspaceUser.PrincipalType.HasValue)
            {
                // Principal type is explicitly set.
                return new GroupUserAccessRight { GroupUserAccessRightProperty = workspaceUser.AccessRight, Identifier = workspaceUser.Identifier, PrincipalType = workspaceUser.PrincipalType.ToString() };
            }
            else
            {
                // Principal type is not set, default to adding a user by their email address.
                return new GroupUserAccessRight { GroupUserAccessRightProperty = workspaceUser.AccessRight, EmailAddress = workspaceUser.UserPrincipalName };
            }
        }
    }
}
