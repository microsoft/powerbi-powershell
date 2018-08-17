/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public interface IDatasetsClient
    {
        object AddDataset(Dataset dataset, Guid? workspaceId = default);

        IEnumerable<Dataset> GetDatasets();
        IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Dataset> GetDatasetsForWorkspace(Guid workspaceId);
        IEnumerable<Dataset> GetDatasetsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null);

        IEnumerable<Datasource> GetDatasources(Guid datasetId, Guid? workspaceId = default);
        IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid datasetId);

        IEnumerable<Table> GetTables(Guid datasetId, Guid? workspaceId = default);

        Table UpdateTable(Table table, Guid datasetId, Guid? workspaceId = default);

        object AddRows(string datasetId, string tableName, JArray rows, Guid? workspaceId = default);
        object DeleteRows(string datasetId, string tableName, Guid? workspaceId = default);
    }
}
