/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Api.Helpers;

namespace Microsoft.PowerBI.Common.Api.Dataflows
{
    public class DataflowsClient : PowerBIEntityClient, IDataflowsClient
    {
        public DataflowsClient(IPowerBIClient client) : base(client)
        {
        }

        public Stream GetDataflow(Guid workspaceId, Guid dataflowId)
        {
            return this.Client.Dataflows.GetDataflow(workspaceId, dataflowId);
        }

        public Stream ExportDataflowAsAdmin(Guid dataflowId)
        {
            return this.Client.Dataflows.ExportDataflowAsAdmin(dataflowId);
        }

        public IEnumerable<Dataflow> GetDataflows(Guid workspaceId)
        {
            return this.Client.Dataflows.GetDataflows(workspaceId).Value?.Select(x => (Dataflow)x);
        }

        public IEnumerable<Dataflow> GetDataflowsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Dataflows.GetDataflowsAsAdmin(filter, top, skip).Value?.Select(x => (Dataflow)x);
        }

        public IEnumerable<Dataflow> GetDataflowsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Dataflows.GetDataflowsInGroupAsAdmin(workspaceId, filter, top, skip).Value?.Select(x => (Dataflow)x);
        }

        public IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid dataflowId)
        {
            return this.Client.Dataflows.GetDatasourcesAsAdmin(dataflowId).Value?.Select(x => (Datasource)x);
        }
    }
}
