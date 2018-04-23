/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;

namespace Microsoft.PowerBI.Common.Api
{
    public interface IPowerBIApiClient
    {
        IReportsClient Reports { get; set; }

        IWorkspacesClient Workspaces { get; set; }
    }
}
