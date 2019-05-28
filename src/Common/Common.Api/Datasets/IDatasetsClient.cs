/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public interface IDatasetsClient
    {
        object AddDataset(Dataset dataset, Guid? workspaceId = default);

        IEnumerable<Dataset> GetDatasets();
        IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = default, int? top = default, int? skip = default, string expand = null);

        IEnumerable<Dataset> GetDatasetsForWorkspace(Guid workspaceId);
        IEnumerable<Dataset> GetDatasetsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null, string expand = null);

        void PatchDataset(Guid datasetId, PatchDatasetRequest patchDatasetRequest, Guid? workspaceId = default);

        IEnumerable<Datasource> GetDatasources(Guid datasetId, Guid? workspaceId = default);
        IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid datasetId);

        IEnumerable<Table> GetTables(Guid datasetId, Guid? workspaceId = default);

        Table UpdateTable(Table table, Guid datasetId, Guid? workspaceId = default);

        object AddRows(string datasetId, string tableName, List<PSObject> rows, Guid? workspaceId = default);
        object DeleteRows(string datasetId, string tableName, Guid? workspaceId = default);
    }
}
