/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.PowerBI.Common.Api.Helpers;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class PatchDatasetRequest
    {
        public DatasetStorageMode TargetStorageMode { get; set; }

        public static implicit operator PatchDatasetRequest(PowerBI.Api.V2.Models.UpdateDatasetRequest patchDatasetRequest)
        {
            if (patchDatasetRequest == null)
            {
                return null;
            }

            return new PatchDatasetRequest
            {
                TargetStorageMode = EnumTypeConverter.ConvertTo<DatasetStorageMode, PowerBI.Api.V2.Models.DatasetStorageMode>(patchDatasetRequest.TargetStorageMode),
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.UpdateDatasetRequest(PatchDatasetRequest patchDatasetRequest)
        {
            if (patchDatasetRequest == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.UpdateDatasetRequest
            {
                TargetStorageMode = EnumTypeConverter.ConvertTo<PowerBI.Api.V2.Models.DatasetStorageMode, DatasetStorageMode>(patchDatasetRequest.TargetStorageMode),
            };
        }
    }
}