/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class WorkspaceRestoreRequest
    {
        public string RestoredName { get; set; }

        public string AdminUserPrincipalName { get; set; }

        public static implicit operator GroupRestoreRequest(WorkspaceRestoreRequest workspaceRestoreRequest)
        {
            return new GroupRestoreRequest { Name = workspaceRestoreRequest.RestoredName, EmailAddress = workspaceRestoreRequest.AdminUserPrincipalName };
        }
    }
}
