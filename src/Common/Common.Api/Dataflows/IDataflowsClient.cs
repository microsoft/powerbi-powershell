/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.PowerBI.Common.Api.Shared;

namespace Microsoft.PowerBI.Common.Api.Dataflows
{
    public interface IDataflowsClient
    {
        IEnumerable<Dataflow> GetDataflows(Guid workspaceId);

        IEnumerable<Dataflow> GetDataflowsAsAdmin(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Dataflow> GetDataflowsAsAdminForWorkspace(Guid workspaceId, string filter = default, int? top = default, int? skip = default);

        IEnumerable<Datasource> GetDataflowDatasources(Guid workspaceId, Guid dataflowId);

        IEnumerable<Datasource> GetDataflowDatasourcesAsAdmin(Guid dataflowId);

        Stream GetDataflow(Guid workspaceId, Guid dataflowId);

        Stream ExportDataflowAsAdmin(Guid dataflowId);

        void DeleteDataflow(Guid workspaceId, Guid dataflowId);

        void RefreshDataflow(Guid workspaceId, Guid dataflowId);
    }
}
