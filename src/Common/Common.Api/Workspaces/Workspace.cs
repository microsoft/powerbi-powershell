/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class Workspace
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool? IsReadOnly { get; set; }

        public bool? IsOnDedicatedCapacity { get; set; }

        public string CapacityId { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string State { get; set; }

        public IEnumerable<WorkspaceUser> Users { get; set; }
    }
}
