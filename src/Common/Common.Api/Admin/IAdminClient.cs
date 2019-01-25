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
        EncryptionKey AddPowerBIEncryptionKey(string name, string keyVaultKeyUri, bool defaultProperty = false, bool activate = false);

        IEnumerable<EncryptionKey> GetPowerBIEncryptionKeys();

        EncryptionKey RotatePowerBIEncryptionKey(string keyObjectId, string keyVaultKeyUri);

        IEnumerable<DatasetEncryptionStatus> GetPowerBIWorkspaceEncryptionStatusInGroup(string groupId);
    }
}
