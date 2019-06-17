/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System;
using System.Collections.Generic;
using Microsoft.PowerBI.Common.Api.Encryption;

namespace Microsoft.PowerBI.Common.Api.Admin
{
    public interface IAdminClient
    {
        EncryptionKey AddPowerBIEncryptionKey(string name, string keyVaultKeyIdentifier, bool? isDefault = null, bool? activate = null);

        IEnumerable<EncryptionKey> GetPowerBIEncryptionKeys();

        IEnumerable<Dataset> GetPowerBIWorkspaceEncryptionStatus(string workspaceId);

        EncryptionKey RotatePowerBIEncryptionKey(Guid tenantKeyId, string keyVaultKeyIdentifier);

        void SetPowerBICapacityEncryptionKey(Guid tenantKeyId, Guid capacityId);
    }
}
