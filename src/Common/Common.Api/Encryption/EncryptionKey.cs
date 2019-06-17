/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */
 
using System;

namespace Microsoft.PowerBI.Common.Api.Encryption
{
    public class EncryptionKey
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri KeyVaultKeyIdentifier { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static implicit operator EncryptionKey(PowerBI.Api.V2.Models.TenantKey tenantKey)
        {
            if (tenantKey == null)
            {
                return null;
            }

            return new EncryptionKey
            {
                Id = new Guid(tenantKey.Id.ToString()),
                Name = tenantKey.Name,
                KeyVaultKeyIdentifier = new Uri(tenantKey.KeyVaultKeyIdentifier),
                IsDefault = tenantKey.IsDefault.Value,
                CreatedAt = tenantKey.CreatedAt.Value,
                UpdatedAt = tenantKey.UpdatedAt.Value,
            };
        }
    }
}
