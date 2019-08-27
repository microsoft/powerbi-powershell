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
using Microsoft.PowerBI.Common.Api.Shared;

namespace Microsoft.PowerBI.Common.Api.Dataflows
{
    public class DataflowsClient : PowerBIEntityClient, IDataflowsClient
    {
        public DataflowsClient(IPowerBIClient client) : base(client)
        {
        }

        public IEnumerable<Dataflow> GetDataflows(Guid workspaceId)
        {
            return this.Client.Dataflows.GetDataflows(workspaceId.ToString()).Value?.Select(x => (Dataflow)x);
        }

        public IEnumerable<Dataflow> GetDataflowsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Dataflows.GetDataflowsAsAdmin(filter, top, skip).Value?.Select(x => (Dataflow)x);
        }

        public IEnumerable<Dataflow> GetDataflowsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Dataflows.GetDataflowsInGroupAsAdmin(workspaceId.ToString(), filter, top, skip).Value?.Select(x => (Dataflow)x);
        }

        public IEnumerable<Datasource> GetDataflowDatasources(Guid workspaceId, Guid dataflowId)
        {
            return this.Client.Dataflows.GetDataflowDataSources(workspaceId.ToString(), dataflowId.ToString()).Value?.Select(x => (Datasource)x);
        }

        public IEnumerable<Datasource> GetDataflowDatasourcesAsAdmin(Guid dataflowId)
        {
            return this.Client.Dataflows.GetDataflowDatasourcesAsAdmin(dataflowId.ToString()).Value?.Select(x => (Datasource)x);
        }

        public Stream GetDataflow(Guid workspaceId, Guid dataflowId)
        {
            return this.Client.Dataflows.GetDataflow(workspaceId.ToString(), dataflowId.ToString());
        }

        public Stream ExportDataflowAsAdmin(Guid dataflowId)
        {
            return this.Client.Dataflows.ExportDataflowAsAdmin(dataflowId.ToString());
        }

        public void DeleteDataflow(Guid workspaceId, Guid dataflowId)
        {
            this.Client.Dataflows.DeleteDataflow(workspaceId.ToString(), dataflowId.ToString());
        }

        public void RefreshDataflow(Guid workspaceId, Guid dataflowId)
        {
            this.Client.Dataflows.RefreshDataflow(workspaceId.ToString(), dataflowId.ToString());
        }
    }
}
