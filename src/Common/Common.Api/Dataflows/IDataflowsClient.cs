/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Dataflows
{
    public interface IDataflowsClient
    {
        Stream GetDataflow(Guid workspaceId, Guid dataflowId);

        Stream ExportDataflowAsAdmin(Guid dataflowId);

        IEnumerable<Dataflow> GetDataflows(Guid workspaceId);

        IEnumerable<Dataflow> GetDataflowsAsAdmin(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Dataflow> GetDataflowsAsAdminForWorkspace(Guid workspaceId, string filter = default, int? top = default, int? skip = default);

        IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid dataflowId);
    }
}
