/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Common.Authentication
{
    public static class Constants
    {
        public static readonly List<string> ApiScopes => new List<string>
        {
            "Dashboard.Read.All",
            "Dashboard.ReadWrite.All",
            "Dataset.Read.All",
            "Dataset.ReadWrite.All",
            "Report.Read.All",
            "Report.ReadWrite.All",
            "Tenant.Read.All",
            "Tenant.ReadWrite.All",
            "Workspace.Read.All",
            "Workspace.ReadWrite.All",
            "Capacity.Read.All",
            "Capacity.ReadWrite.All",
            "Gateway.Read.All",
            "Gateway.ReadWrite.All",
            "App.Read.All",
            "StorageAccount.Read.All",
            "StorageAccount.ReadWrite.All",
            "Dataflow.Read.All",
            "Dataflow.ReadWrite.All",
            "Content.Create",
            "Pipeline.Deploy",
            "Pipeline.Read.All",
            "Pipeline.ReadWrite.All",
            "UserState.ReadWrite.All",
        };
    }
}