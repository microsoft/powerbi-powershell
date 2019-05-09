/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Common.Api.Helpers;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Dataset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConfiguredBy { get; set; }
        public DefaultRetentionPolicy? DefaultRetentionPolicy { get; set; }
        public bool AddRowsApiEnabled { get; set; }
        public IEnumerable<Table> Tables { get; set; }
        public string WebUrl { get; set; }
        public IEnumerable<Relationship> Relationships { get; set; }
        public IEnumerable<Datasource> Datasources { get; set; }
        public DatasetMode? DefaultMode { get; set; }
        public bool IsRefreshable { get; set; }
        public bool IsEffectiveIdentityRequired { get; set; }
        public bool IsEffectiveIdentityRolesRequired { get; set; }
        public bool IsOnPremGatewayRequired { get; set; }

        public static implicit operator Dataset(PowerBI.Api.V2.Models.Dataset dataset)
        {
            if(dataset == null)
            {
                return null;
            }

            return new Dataset
            {
                Id = dataset.Id,
                Name = dataset.Name,
                ConfiguredBy = dataset.ConfiguredBy,
                AddRowsApiEnabled = dataset.AddRowsAPIEnabled.GetValueOrDefault(),
                WebUrl = dataset.WebUrl,
                IsRefreshable = dataset.IsRefreshable.GetValueOrDefault(),
                IsEffectiveIdentityRequired = dataset.IsEffectiveIdentityRequired.GetValueOrDefault(),
                IsOnPremGatewayRequired = dataset.IsOnPremGatewayRequired.GetValueOrDefault()
            };
        }

        public static PowerBI.Api.V2.Models.CreateDatasetRequest ConvertToDatasetRequest(Dataset dataset)
        {
            if (dataset == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.CreateDatasetRequest
            {
                Name = dataset.Name,
                Tables = dataset.Tables?.Select(t => (PowerBI.Api.V2.Models.Table)t).ToList(),
                Datasources = dataset.Datasources?.Select(d => (PowerBI.Api.V2.Models.Datasource)d).ToList(),
                Relationships = dataset.Relationships?.Select(r => (PowerBI.Api.V2.Models.Relationship)r).ToList(),
                DefaultMode = EnumTypeConverter.ConvertTo<PowerBI.Api.V2.Models.DatasetMode, DatasetMode>(dataset.DefaultMode)
            };
        }

    }

    public enum DefaultRetentionPolicy
    {
        None,
        BasicFIFO
    }

    public enum DatasetMode
    {
        AsAzure,
        AsOnPrem,
        Push,
        Streaming,
        PushStreaming
    }
}
