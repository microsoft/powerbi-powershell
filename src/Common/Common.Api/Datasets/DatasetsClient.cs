/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Common.Api.Shared;

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

        public IEnumerable<Dataset> GetDatasets(string expand = null)
        {
            return this.Client.Datasets.GetDatasetsWithODataQuery(expand).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsForWorkspace(Guid workspaceId, string expand = null)
        {
            return this.Client.Datasets.GetDatasetsWithODataQuery(groupId: workspaceId.ToString(), expand: expand).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = null, int? top = null, int? skip = null, string expand = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(filter: filter, top: top, skip: skip, expand: expand).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null, string expand = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(groupId: workspaceId.ToString(), filter: filter, top: top, skip: skip, expand: expand).Value?.Select(x => (Dataset)x);
        }

        public void PatchDataset(Guid datasetId, PatchDatasetRequest patchDatasetRequest, Guid? workspaceId = default)
        {
            if (workspaceId.HasValue && workspaceId.Value != default)
            {
                this.Client.Datasets.UpdateDatasetByIdInGroup(groupId: workspaceId.Value.ToString(), datasetKey: datasetId.ToString(), updateDatasetRequest: patchDatasetRequest);
            }
            else
            {
                this.Client.Datasets.UpdateDatasetById(datasetKey: datasetId.ToString(), updateDatasetRequest: patchDatasetRequest);
            }
        }

        public object DeleteDataset(Guid datasetId, Guid? workspaceId = default)
        {
            if (workspaceId.HasValue && workspaceId.Value != default)
            {
                return this.Client.Datasets.DeleteDatasetByIdInGroup(groupId: workspaceId.Value.ToString(), datasetKey: datasetId.ToString());
            }
            else
            {
                return this.Client.Datasets.DeleteDatasetById(datasetKey: datasetId.ToString());
            }
        }

        public object DeleteDataset(Guid datasetId)
        {
            return this.Client.Datasets.DeleteDatasetById(datasetKey: datasetId.ToString());
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

        public object AddRows(string datasetId, string tableName, List<PSObject> rows, Guid? workspaceId = default)
        {
            var hashRows = new List<Hashtable>();
            foreach (var row in rows)
            {
                var hashtable = new Hashtable();
                if (row.BaseObject is Hashtable)
                {
                    foreach (DictionaryEntry baseObj in (row.BaseObject as Hashtable))
                    {
                        hashtable.Add(baseObj.Key, baseObj.Value);
                    }
                }
                else
                {
                    foreach (var member in row.Members.Where(x => x is PSNoteProperty))
                    {
                        hashtable.Add(member.Name, member.Value);
                    }
                }
                hashRows.Add(hashtable);
            }
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.PostRowsInGroup(groupId: workspaceId.Value.ToString(), datasetKey: datasetId, tableName: tableName, requestMessage: hashRows) :
                this.Client.Datasets.PostRows(datasetKey: datasetId, tableName: tableName, requestMessage: hashRows);

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
