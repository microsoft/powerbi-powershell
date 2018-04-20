/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class WorkspaceUser
    {
        public string AccessRight { get; set; }

        public string UserPrincipalName { get; set; }

        public static implicit operator WorkspaceUser(GroupUserAccessRight groupUserAccessRight)
        {
            return new WorkspaceUser { AccessRight = groupUserAccessRight.GroupUserAccessRightProperty, UserPrincipalName = groupUserAccessRight.EmailAddress };
        }

        public static implicit operator GroupUserAccessRight(WorkspaceUser workspaceUser)
        {
            return new GroupUserAccessRight { GroupUserAccessRightProperty = workspaceUser.AccessRight, EmailAddress = workspaceUser.UserPrincipalName };
        }
    }
}
