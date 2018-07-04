/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Microsoft.PowerBI.Api.V2;
using System.Linq;

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
             this.Client.Datasets.PostDatasetInGroup(groupId: workspaceId.Value.ToString(), dataset: dataset) :            
            this.Client.Datasets.PostDataset(dataset: dataset) ;

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
    }
}
