/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Common.Api;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName)]
    public class RotatePowerBIEncryptionKey : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = "Rotate";

        public RotatePowerBIEncryptionKey() : base() { }

        public RotatePowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string KeyVaultKeyUri { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                try
                {
                    var tenantKeys = this.GetEncryptionKeys(client);
                    var matchedencryptionKey = this.GetMatchingEncryptionKey(tenantKeys);
                    var response = client.Admin.RotatePowerBIEncryptionKey(matchedencryptionKey.Id.ToString(), KeyVaultKeyUri);

                    this.Logger.WriteObject(response);
                }
                catch (Exception ex)
                {
                    this.Logger.ThrowTerminatingError(ex);
                }
            }
        }

        private IEnumerable<TenantKey> GetEncryptionKeys(IPowerBIApiClient client)
        {
            var tenantKeys = client.Admin.GetPowerBIEncryptionKeys();
            if (tenantKeys == null)
            {
                throw new Exception("No encryption keys is set");
            }

            return tenantKeys;
        }

        private TenantKey GetMatchingEncryptionKey(IEnumerable<TenantKey> encryptionKeys)
        {
            var matchedencryptionKey = encryptionKeys.FirstOrDefault(
                    (encryptionKey) => encryptionKey.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            if (matchedencryptionKey == default(TenantKey))
            {
                throw new Exception("No matching encryption keys found");
            }

            return matchedencryptionKey;
        }
    }
}
