/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Admin
{
    public class AdminClient : PowerBIEntityClient, IAdminClient
    {
        public AdminClient(IPowerBIClient client): base(client) { }

        public EncryptionKey AddPowerBIEncryptionKey(string name, string keyVaultKeyUri, bool defaultProperty = false, bool activate = false)
        {
            var tenentKeysRequest = new TenentKeysRequest()
            {
                Name = name,
                KeyVaultKeyUri = keyVaultKeyUri,
                DefaultProperty = defaultProperty,
                Activate = activate
            };

            return this.Client.Admin.AddPowerBIEncryptionKey(tenentKeysRequest);
        }

        public IEnumerable<EncryptionKey> GetPowerBIEncryptionKeys()
        {
            return this.Client.Admin.GetPowerBIEncryptionKeys()?.Value;
        }

        public EncryptionKey RotatePowerBIEncryptionKey(string keyObjectId, string keyVaultKeyUri)
        {
            var rotateKeyRequest = new RotateKeysRequest(keyVaultKeyUri);
            return this.Client.Admin.RotatePowerBIEncryptionKey(keyObjectId, rotateKeyRequest);
        }

        public IEnumerable<DatasetEncryptionStatus> GetPowerBIWorkspaceEncryptionStatusInGroup(string groupId)
        {
            return this.Client.Admin.GetPowerBIWorkspaceEncryptionStatusInGroup(groupId)?.Value;
        }
    }
}
