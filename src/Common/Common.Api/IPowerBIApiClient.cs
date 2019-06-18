/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Common.Api.Admin;
using Microsoft.PowerBI.Common.Api.Capacities;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Gateways.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Api.Dataflows;

namespace Microsoft.PowerBI.Common.Api
{
    public interface IPowerBIApiClient : IDisposable
    {
        IReportsClient Reports { get; set; }

        IWorkspacesClient Workspaces { get; set; }

        IDatasetsClient Datasets { get; set; }
        
        IAdminClient Admin { get; set; }

        ICapacityClient Capacities { get; set; }

        IGatewayClient Gateways { get; set; }

        IDataflowsClient Dataflows { get; set; }
    }
}
