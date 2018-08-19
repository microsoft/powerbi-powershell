/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class DatasetsClient : PowerBIEntityClient, IDatasetsClient
    {
        public DatasetsClient(IPowerBIClient client) : base(client)
        {
        }

        public object AddDataset(Dataset dataset, Guid? workspaceId = default)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.PostDatasetInGroup(groupId: workspaceId.Value.ToString(), dataset: Dataset.ConvertToDatasetV2Model(dataset)) :
                this.Client.Datasets.PostDataset(dataset: Dataset.ConvertToDatasetV2Model(dataset));

            return result;
        }

        public IEnumerable<Dataset> GetDatasets()
        {
            return this.Client.Datasets.GetDatasets().Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsForWorkspace(Guid workspaceId)
        {
            return this.Client.Datasets.GetDatasets(groupId: workspaceId.ToString()).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(filter: filter, top: top, skip: skip).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(groupId: workspaceId.ToString(), filter: filter, top: top, skip: skip).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Datasource> GetDatasources(Guid datasetId, Guid? workspaceId = default)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.GetDatasources(groupId: workspaceId.Value.ToString(), datasetKey: datasetId.ToString()) :
                this.Client.Datasets.GetDatasources(datasetKey: datasetId.ToString());
            return result.Value?.Select(x => (Datasource)x);

        }

        public IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid datasetId)
        {
            return this.Client.Datasets.GetDatasourcesAsAdmin(datasetId.ToString()).Value?.Select(x => (Datasource)x);
        }

        public IEnumerable<Table> GetTables(Guid datasetId, Guid? workspaceId = default)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.GetTables(groupId: workspaceId.Value.ToString(), datasetKey: datasetId.ToString()) :
                this.Client.Datasets.GetTables(datasetKey: datasetId.ToString());
            return result.Value?.Select(x => (Table)x);
        }

        public Table UpdateTable(Table table, Guid datasetId, Guid? workspaceId = null)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.PutTableInGroup(groupId: workspaceId.Value.ToString(), datasetKey: datasetId.ToString(), tableName: table.Name, (Microsoft.PowerBI.Api.V2.Models.Table)table) :
                this.Client.Datasets.PutTable(datasetKey: datasetId.ToString(), tableName: table.Name, (Microsoft.PowerBI.Api.V2.Models.Table)table);
            return result as Table;
        }

        public object AddRows(string datasetId, string tableName, List<Hashtable> rows, Guid? workspaceId = default)
        {
            
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.PostRowsInGroup(groupId: workspaceId.Value.ToString(), datasetKey: datasetId, tableName: tableName, requestMessage: rows) :
                this.Client.Datasets.PostRows(datasetKey: datasetId, tableName: tableName, requestMessage: rows);

            return result;
        }

        public object DeleteRows(string datasetId, string tableName, Guid? workspaceId = default)
        {

            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.DeleteRowsInGroup(groupId: workspaceId.Value.ToString(), datasetKey: datasetId, tableName: tableName) :
                this.Client.Datasets.DeleteRows(datasetKey: datasetId, tableName: tableName);

            return result;
        }
    }
}
