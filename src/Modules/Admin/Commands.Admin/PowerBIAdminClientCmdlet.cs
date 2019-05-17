/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    public abstract class PowerBIAdminClientCmdlet : PowerBIClientCmdlet
    {
        public PowerBIAdminClientCmdlet() : base() { }

        public PowerBIAdminClientCmdlet(IPowerBIClientCmdletInitFactory init) : base(init) { }

        protected EncryptionKey GetEncryptionKey(IPowerBIApiClient client, string keyName)
        {
            var tenantKeys = this.GetEncryptionKeys(client);
            if (tenantKeys == null)
            {
                return null;
            }

            return GetMatchingEncryptionKey(tenantKeys, keyName);
        }

        private IEnumerable<EncryptionKey> GetEncryptionKeys(IPowerBIApiClient client)
        {
            var tenantKeys = client.Admin.GetPowerBIEncryptionKeys();
            if (tenantKeys == null || !tenantKeys.Any())
            {
                this.Logger.ThrowTerminatingError("No encryption keys are set");
                return null;
            }

            return tenantKeys;
        }

        private EncryptionKey GetMatchingEncryptionKey(IEnumerable<EncryptionKey> encryptionKeys, string keyName)
        {
            var matchedEncryptionKey = encryptionKeys.FirstOrDefault(
                    (encryptionKey) => encryptionKey.Name.Equals(keyName, StringComparison.OrdinalIgnoreCase));
            if (matchedEncryptionKey == default(EncryptionKey))
            {
                this.Logger.ThrowTerminatingError("No matching encryption keys found");
                return null;
            }

            return matchedEncryptionKey;
        }
    }
}
