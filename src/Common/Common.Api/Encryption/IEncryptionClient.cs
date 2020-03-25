/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Encryption
{
    public interface IEncryptionClient
    {
        EncryptionKey AddPowerBIEncryptionKey(string name, string keyVaultKeyIdentifier, bool? isDefault = null, bool? activate = null);

        IEnumerable<EncryptionKey> GetPowerBIEncryptionKeys();

        EncryptionKey RotatePowerBIEncryptionKey(Guid tenantKeyId, string keyVaultKeyIdentifier);

        void SetPowerBICapacityEncryptionKey(Guid tenantKeyId, Guid capacityId);
    }
}
