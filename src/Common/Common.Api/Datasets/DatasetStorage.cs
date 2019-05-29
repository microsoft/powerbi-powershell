/*
 * Copyright(c) Microsoft Corporation.All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Api.Helpers;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class DatasetStorage
    {
        public string Id { get; set; }
        public DatasetStorageMode StorageMode { get; set; }

        public static implicit operator DatasetStorage(PowerBI.Api.V2.Models.DatasetStorage datasetStorage)
        {
            if (datasetStorage == null)
            {
                return null;                                                                                                                                                                                                                            }

            return new DatasetStorage
            {
                Id = datasetStorage.Id,
                StorageMode = EnumTypeConverter.ConvertTo<DatasetStorageMode, PowerBI.Api.V2.Models.DatasetStorageMode>(datasetStorage.StorageMode),
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.DatasetStorage(DatasetStorage datasetStorage)
        {
            if (datasetStorage == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.DatasetStorage
            {
                Id = datasetStorage.Id,
                StorageMode = EnumTypeConverter.ConvertTo<PowerBI.Api.V2.Models.DatasetStorageMode, DatasetStorageMode>(datasetStorage.StorageMode),
            };
        }
    }

    public enum DatasetStorageMode
    {
        Unknown,
        Abf,
        PremiumFiles,
    }
}