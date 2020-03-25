/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Encryption
{
    public class EncryptionClient : PowerBIEntityClient, IEncryptionClient
    {
        public EncryptionClient(IPowerBIClient client): base(client) { }

        public EncryptionKey AddPowerBIEncryptionKey(string name, string keyVaultKeyIdentifier, bool? isDefault = null, bool? activate = null)
        {
            var tenantKeyCreationRequest = new TenantKeyCreationRequest()
            {
                Name = name,
                KeyVaultKeyIdentifier = keyVaultKeyIdentifier,
                IsDefault = isDefault,
                Activate = activate
            };

            return this.Client.EncryptionKeys.AddPowerBIEncryptionKey(tenantKeyCreationRequest);
        }

        public IEnumerable<EncryptionKey> GetPowerBIEncryptionKeys()
        {
            return this.Client.EncryptionKeys.GetPowerBIEncryptionKeys()?.Value
                    .Select(key => (EncryptionKey)key);
        }

        public EncryptionKey RotatePowerBIEncryptionKey(Guid tenantKeyId, string keyVaultKeyIdentifier)
        {
            var tenantKeyRotationRequest = new TenantKeyRotationRequest()
            {
                KeyVaultKeyIdentifier = keyVaultKeyIdentifier
            };

            return this.Client.EncryptionKeys.RotatePowerBIEncryptionKey(tenantKeyId, tenantKeyRotationRequest);
        }

        public void SetPowerBICapacityEncryptionKey(Guid tenantKeyId, Guid capacityId)
        {
            var capacityPatchRequest = new CapacityPatchRequest()
            {
                TenantKeyId = tenantKeyId
            };

            this.Client.EncryptionKeys.PatchCapacity(capacityId, capacityPatchRequest);
        }
    }
}
