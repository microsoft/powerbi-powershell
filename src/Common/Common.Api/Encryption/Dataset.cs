/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Api.Encryption
{
    public class Dataset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EncryptionStatus EncryptionStatus { get; set; }

        public static implicit operator Dataset(PowerBI.Api.V2.Models.Dataset dataset)
        {
            if (dataset == null)
            {
                return null;
            }

            return new Dataset
            {
                Id = dataset.Id,
                Name = dataset.Name,
                EncryptionStatus = (EncryptionStatus)dataset.Encryption.EncryptionStatus
            };
        }
    }

    public enum EncryptionStatus
    {
        Unknown = 0,
        NotSupported = 1,
        InSyncWithWorkspace = 2,
        NotInSyncWithWorkspace = 3
    }
}
