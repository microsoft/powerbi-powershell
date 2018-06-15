using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Dataset
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ConfiguredBy { get; set; }
        public string DefaultRetentionPolicy { get; set; }
        public bool AddRowsApiEnabled { get; set; }
        public IEnumerable<Table> Tables { get; set; }
        public string WebUrl { get; set; }
        public IEnumerable<Relationship> Relationships { get; set; }
        public IEnumerable<Datasource> Datasources { get; set; }
        public DatasetMode DefaultMode { get; set; }
        public bool IsRefreshable { get; set; }
        public bool IsEffectiveIdentityRequired { get; set; }
        public bool IsEffectiveIdentityRolesRequired { get; set; }
        public bool IsOnPremGatewayRequired { get; set; }

        public static implicit operator Dataset(PowerBI.Api.V2.Models.Dataset dataset)
        {
            return new Dataset
            {
                Id = dataset.Id,
                Name = dataset.Name,
                ConfiguredBy = dataset.ConfiguredBy,
                DefaultRetentionPolicy = dataset.DefaultRetentionPolicy,
                AddRowsApiEnabled = dataset.AddRowsAPIEnabled.GetValueOrDefault(),
                Tables = dataset.Tables?.Select(t => (Table)t),
                WebUrl = dataset.WebUrl,
                Relationships = dataset.Relationships?.Select(r => (Relationship)r),
                Datasources = dataset.Datasources?.Select(d => (Datasource)d),
                DefaultMode = (DatasetMode)Enum.Parse(typeof(DatasetMode), dataset.DefaultMode),
                IsRefreshable = dataset.IsRefreshable.GetValueOrDefault(),
                IsEffectiveIdentityRequired = dataset.IsEffectiveIdentityRequired.GetValueOrDefault(),
                IsOnPremGatewayRequired = dataset.IsOnPremGatewayRequired.GetValueOrDefault()
            };
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
