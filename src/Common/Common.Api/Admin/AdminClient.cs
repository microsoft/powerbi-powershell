/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Admin
{
    public class AdminClient : PowerBIEntityClient, IAdminClient
    {
        public AdminClient(IPowerBIClient client): base(client) { }

        public TenantKey AddPowerBIEncryptionKey(string name, string keyVaultKeyIdentifier, bool? isDefault = null, bool? activate = null)
        {
            var tenantKeyCreationRequest = new TenantKeyCreationRequest()
            {
                Name = name,
                KeyVaultKeyIdentifier = keyVaultKeyIdentifier,
                IsDefault = isDefault,
                Activate = activate
            };

            return this.Client.Admin.AddPowerBIEncryptionKey(tenantKeyCreationRequest);
        }

        public IEnumerable<TenantKey> GetPowerBIEncryptionKeys()
        {
            return this.Client.Admin.GetPowerBIEncryptionKeys()?.Value;
        }

        public IEnumerable<Dataset> GetPowerBIWorkspaceEncryptionStatus(string workspaceId)
        {
            return this.Client.Datasets.GetDatasetsInGroupAsAdmin(workspaceId, expand: "encryption")?.Value;
        }

        public TenantKey RotatePowerBIEncryptionKey(string tenantKeyId, string keyVaultKeyIdentifier)
        {
            var tenantKeyRotationRequest = new TenantKeyRotationRequest()
            {
                KeyVaultKeyIdentifier = keyVaultKeyIdentifier
            };

            return this.Client.Admin.RotatePowerBIEncryptionKey(Guid.Parse(tenantKeyId), tenantKeyRotationRequest);
        }
    }
}
