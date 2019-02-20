/*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT License.
*/

using System.Collections.Generic;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Admin
{
    public interface IAdminClient
    {
        TenantKey AddPowerBIEncryptionKey(string name, string keyVaultKeyIdentifier, bool? isDefault = null, bool? activate = null);

        IEnumerable<TenantKey> GetPowerBIEncryptionKeys();

        IEnumerable<Dataset> GetPowerBIWorkspaceEncryptionStatus(string workspaceId);

        TenantKey RotatePowerBIEncryptionKey(string tenantKeyId, string keyVaultKeyIdentifier);
    }
}
