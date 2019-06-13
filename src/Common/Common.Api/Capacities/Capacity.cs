/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Microsoft.PowerBI.Common.Api.Encryption;

namespace Microsoft.PowerBI.Common.Api.Capacities
{
    public class Capacity
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public IEnumerable<string> Admins { get; set; }

        public string Sku { get; set; }

        public CapacityState State { get; set; }

        public CapacityUserAccessRight UserAccessRight { get; set; }

        public string Region { get; set; }

        public Guid? EncryptionKeyId { get; set; }

        public EncryptionKey EncryptionKey { get; set; }

        public static implicit operator Capacity(PowerBI.Api.V2.Models.Capacity capacity)
        {
            if (capacity == null)
            {
                return null;
            }

            return new Capacity
            {
                Id = Guid.Parse(capacity.Id),
                DisplayName = capacity.DisplayName,
                Admins = capacity.Admins,
                Sku = capacity.Sku,
                State = (CapacityState)Enum.Parse(typeof(CapacityState), capacity.State, ignoreCase: true),
                UserAccessRight = (CapacityUserAccessRight)Enum.Parse(
                    typeof(CapacityUserAccessRight), capacity.CapacityUserAccessRight, ignoreCase: true),
                Region = capacity.Region,
                EncryptionKeyId = capacity.TenantKeyId,
                EncryptionKey = capacity.TenantKey
            };
        }

        public enum CapacityState
        {
            NotActivated = 0,
            Active = 1,
            Provisioning = 2,
            ProvisionFailed = 3,
            Suspended = 4,
            PreSuspended = 5,
            Deleting = 6,
            Deleted = 7,
            Invalid = 8,
            UpdatingSku = 9
        }

        public enum CapacityUserAccessRight
        {
            None = 0,
            Assign = 1,
            Admin = 2
        }
    }
}
