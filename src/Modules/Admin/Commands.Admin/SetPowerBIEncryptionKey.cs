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
    [Alias("Rotate-PowerBIEncryptionKey")]
    [OutputType(typeof(TenantKey))]
    public class SetPowerBIEncryptionKey : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBIEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Set;

        public SetPowerBIEncryptionKey() : base() { }

        public SetPowerBIEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string KeyVaultKeyUri { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            this.Logger.WriteWarning(Constants.PrivatePreviewWarning);
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var tenantKeys = this.GetEncryptionKeys(client);
                if (tenantKeys == null)
                {
                    // Return for test cases where no tenant keys are found
                    return;
                }

                var matchedencryptionKey = this.GetMatchingEncryptionKey(tenantKeys);
                if (matchedencryptionKey == null)
                {
                    // Return for test cases where no matching encryption keys are found
                    return;
                }

                var response = client.Admin.RotatePowerBIEncryptionKey(matchedencryptionKey.Id.ToString(), KeyVaultKeyUri);

                this.Logger.WriteObject(response);
            }
        }

        private IEnumerable<TenantKey> GetEncryptionKeys(IPowerBIApiClient client)
        {
            var tenantKeys = client.Admin.GetPowerBIEncryptionKeys();
            if (tenantKeys == null || !tenantKeys.Any())
            {
                this.Logger.ThrowTerminatingError("No encryption keys are set");
                return null;
            }

            return tenantKeys;
        }

        private TenantKey GetMatchingEncryptionKey(IEnumerable<TenantKey> encryptionKeys)
        {
            var matchedencryptionKey = encryptionKeys.FirstOrDefault(
                    (encryptionKey) => encryptionKey.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            if (matchedencryptionKey == default(TenantKey))
            {
                this.Logger.ThrowTerminatingError("No matching encryption keys found");
                return null;
            }

            return matchedencryptionKey;
        }
    }
}
