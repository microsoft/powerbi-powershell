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
                IsOnPremGatewayRequired = dataset.IsOnPremGatewayRequired.GetValueOrDefault()
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Dataset(Dataset dataset)
        {
            if (dataset == null)
            {
                return null;
            }

            // We can assign all properties to dataset but some values prevent from creating push dataset, thus I didn't set them.
            return new PowerBI.Api.V2.Models.Dataset
            {
                Id = dataset.Id == Guid.Empty ? null : dataset.Id.ToString(),
                Name = dataset.Name,
                // ConfiguredBy = dataset.ConfiguredBy,
                // DefaultRetentionPolicy = dataset.DefaultRetentionPolicy,
                // AddRowsAPIEnabled = dataset.AddRowsApiEnabled,
                Tables = dataset.Tables?.Select(t => (PowerBI.Api.V2.Models.Table)t).ToList(),
                // WebUrl = dataset.WebUrl,
                // Relationships = dataset.Relationships?.Select(r => (PowerBI.Api.V2.Models.Relationship)r).ToList(),
                // Datasources = dataset.Datasources?.Select(d => (PowerBI.Api.V2.Models.Datasource)d).ToList(),
                // DefaultMode = dataset.DefaultMode.ToString(),
                // IsRefreshable = dataset.IsRefreshable,
                // IsEffectiveIdentityRequired = dataset.IsEffectiveIdentityRequired,
                // IsOnPremGatewayRequired = dataset.IsOnPremGatewayRequired
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
