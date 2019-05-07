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
using Microsoft.PowerBI.Common.Api.Helpers;

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
                this.Client.Datasets.PostDatasetInGroup(groupId: workspaceId.Value, dataset: Dataset.ConvertToDatasetRequest(dataset), EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.DefaultRetentionPolicy, DefaultRetentionPolicy>(dataset.DefaultRetentionPolicy)) :
                this.Client.Datasets.PostDataset(dataset: Dataset.ConvertToDatasetRequest(dataset), EnumTypeConverter.ConvertTo<Microsoft.PowerBI.Api.V2.Models.DefaultRetentionPolicy, DefaultRetentionPolicy>(dataset.DefaultRetentionPolicy));

            return result;
        }

        public IEnumerable<Dataset> GetDatasets()
        {
            return this.Client.Datasets.GetDatasets().Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsForWorkspace(Guid workspaceId)
        {
            return this.Client.Datasets.GetDatasets(groupId: workspaceId).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(filter: filter, top: top, skip: skip).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Dataset> GetDatasetsAsAdminForWorkspace(Guid workspaceId, string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Datasets.GetDatasetsAsAdmin(groupId: workspaceId, filter: filter, top: top, skip: skip).Value?.Select(x => (Dataset)x);
        }

        public IEnumerable<Datasource> GetDatasources(Guid datasetId, Guid? workspaceId = default)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.GetDatasources(groupId: workspaceId.Value, datasetId: datasetId) :
                this.Client.Datasets.GetDatasources(datasetId: datasetId);
            return result.Value?.Select(x => (Datasource)x);

        }

        public IEnumerable<Datasource> GetDatasourcesAsAdmin(Guid datasetId)
        {
            return this.Client.Datasets.GetDatasourcesAsAdmin(datasetId).Value?.Select(x => (Datasource)x);
        }

        public IEnumerable<Table> GetTables(Guid datasetId, Guid? workspaceId = default)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.GetTables(groupId: workspaceId.Value, datasetId: datasetId) :
                this.Client.Datasets.GetTables(datasetId: datasetId);
            return result.Value?.Select(x => (Table)x);
        }

        public Table UpdateTable(Table table, Guid datasetId, Guid? workspaceId = null)
        {
            var result = workspaceId.HasValue && workspaceId.Value != default ?
                this.Client.Datasets.PutTableInGroup(groupId: workspaceId.Value, datasetId: datasetId, tableName: table.Name, (Microsoft.PowerBI.Api.V2.Models.Table)table) :
                this.Client.Datasets.PutTable(datasetId: datasetId, tableName: table.Name, (Microsoft.PowerBI.Api.V2.Models.Table)table);

            if (result != null)
            {
                return (Table)result;
            }
            else
            {
                return null;
            }
        }

        public void AddRows(Guid datasetId, string tableName, List<PSObject> rows, Guid? workspaceId = null)
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

            if(workspaceId.HasValue && workspaceId.Value != Guid.Empty)
            {
                this.Client.Datasets.PostRowsInGroup(groupId: workspaceId.Value, datasetId: datasetId, tableName: tableName, requestMessage: hashRows);
            }
            else
            {
                this.Client.Datasets.PostRows(datasetId: datasetId, tableName: tableName, requestMessage: hashRows);
            } 
        }

        public void DeleteRows(Guid datasetId, string tableName, Guid? workspaceId = default)
        {
            if(workspaceId.HasValue && workspaceId.Value != default)
            {
                this.Client.Datasets.DeleteRowsInGroup(groupId: workspaceId.Value, datasetId: datasetId, tableName: tableName);
            }
            else
            {
                this.Client.Datasets.DeleteRows(datasetId: datasetId, tableName: tableName);
            }   
        }
    }
}
