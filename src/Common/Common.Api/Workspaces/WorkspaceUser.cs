/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Api.Helpers;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class WorkspaceUser
    {
        public WorkspaceUserAccessRight AccessRight { get; set; }

        public string UserPrincipalName { get; set; }

        public string DisplayName { get; set; }

        public string Identifier { get; set; }

        public PrincipalType? PrincipalType { get; set; }

        public static implicit operator WorkspaceUser(Microsoft.PowerBI.Api.V2.Models.GroupUser groupUser)
        {
            return new WorkspaceUser
            {
                AccessRight = EnumTypeConverter.ConvertTo<WorkspaceUserAccessRight, Microsoft.PowerBI.Api.V2.Models.GroupUserAccessRight>(groupUser.GroupUserAccessRight),
                UserPrincipalName = groupUser.EmailAddress,
                DisplayName = groupUser.DisplayName,
                Identifier = groupUser.Identifier,
                PrincipalType = EnumTypeConverter.ConvertTo<PrincipalType, Microsoft.PowerBI.Api.V2.Models.PrincipalType>(groupUser.PrincipalType)
            };
        }

        public static implicit operator Microsoft.PowerBI.Api.V2.Models.GroupUser(WorkspaceUser workspaceUser)
        {
            return new Microsoft.PowerBI.Api.V2.Models.GroupUser
            {
                GroupUserAccessRight = EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.GroupUserAccessRight, WorkspaceUserAccessRight>(workspaceUser.AccessRight),
                EmailAddress = workspaceUser.UserPrincipalName,
                DisplayName = workspaceUser.DisplayName,
                Identifier = workspaceUser.Identifier,
                PrincipalType = EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.PrincipalType, PrincipalType>(workspaceUser.PrincipalType)
            };
        }
    }
}
