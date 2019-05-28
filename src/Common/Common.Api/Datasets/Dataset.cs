/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Dataset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConfiguredBy { get; set; }
        public string DefaultRetentionPolicy { get; set; }
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
        public DatasetStorageMode? TargetStorageMode { get; set; }
        public DatasetStorage ActualStorage { get; set; }

        public static implicit operator Dataset(PowerBI.Api.V2.Models.Dataset dataset)
        {
            if(dataset == null)
            {
                return null;
            }

            return new Dataset
            {
                Id = new Guid(dataset.Id),
                Name = dataset.Name,
                ConfiguredBy = dataset.ConfiguredBy,
                DefaultRetentionPolicy = dataset.DefaultRetentionPolicy,
                AddRowsApiEnabled = dataset.AddRowsAPIEnabled.GetValueOrDefault(),
                Tables = dataset.Tables?.Select(t => (Table)t),
                WebUrl = dataset.WebUrl,
                Relationships = dataset.Relationships?.Select(r => (Relationship)r),
                Datasources = dataset.Datasources?.Select(d => (Datasource)d),
                DefaultMode = ConvertDefaultMode(dataset.DefaultMode),
                IsRefreshable = dataset.IsRefreshable.GetValueOrDefault(),
                IsEffectiveIdentityRequired = dataset.IsEffectiveIdentityRequired.GetValueOrDefault(),
                IsOnPremGatewayRequired = dataset.IsOnPremGatewayRequired.GetValueOrDefault(),
                TargetStorageMode = EnumTypeConverter.ConvertTo<DatasetStorageMode, PowerBI.Api.V2.Models.DatasetStorageMode>(dataset.TargetStorageMode),
                ActualStorage = dataset.ActualStorage,
            };
        }

        public static PowerBI.Api.V2.Models.Dataset ConvertToDatasetV2Model(Dataset dataset)
        {
            if (dataset == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Dataset
            {
                Id = dataset.Id == Guid.Empty ? null : dataset.Id.ToString(),
                Name = dataset.Name,
                Tables = dataset.Tables?.Select(t => (PowerBI.Api.V2.Models.Table)t).ToList(),
            };
        }

        private static DatasetMode? ConvertDefaultMode(string defaultMode)
        {
            if(string.IsNullOrEmpty(defaultMode))
            {
                return null;
            }

            return (DatasetMode)Enum.Parse(typeof(DatasetMode), defaultMode, true);
        }
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
