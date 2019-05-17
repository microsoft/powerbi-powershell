/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Capacities;
using Microsoft.PowerBI.Common.Api.Encryption;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Admin
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = KeyNameAndCapacityIdParameterSet)]
    public class SetPowerBICapacityEncryptionKey : PowerBIAdminClientCmdlet
    {
        public const string CmdletName = "PowerBICapacityEncryptionKey";
        public const string CmdletVerb = VerbsCommon.Set;

        public SetPowerBICapacityEncryptionKey() : base() { }

        public SetPowerBICapacityEncryptionKey(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameter set names
        public const string KeyNameAndCapacityIdParameterSet = "KeyNameAndCapacityId";
        public const string KeyNameAndCapacityParameterSet = "KeyNameAndCapacity";
        public const string KeyAndCapacityIdParameterSet = "KeyAndCapacityId";
        #endregion

        #region Parameters

        [Parameter(Mandatory = true, ParameterSetName = KeyNameAndCapacityIdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = KeyNameAndCapacityParameterSet)]
        public string KeyName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = KeyAndCapacityIdParameterSet, ValueFromPipeline = true)]
        public EncryptionKey Key { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = KeyNameAndCapacityIdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = KeyAndCapacityIdParameterSet)]
        public Guid CapacityId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = KeyNameAndCapacityParameterSet, ValueFromPipeline = true)]
        public Capacity Capacity { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                switch (ParameterSet)
                {
                    case KeyNameAndCapacityIdParameterSet:
                        break;
                    case KeyNameAndCapacityParameterSet:
                        CapacityId = Capacity.Id;
                        break;
                    case KeyAndCapacityIdParameterSet:
                        KeyName = Key.Name;
                        break;
                }
                
                var encryptionKey = GetEncryptionKey(client, KeyName);
                if (encryptionKey == null)
                {
                    return;
                }

                client.Admin.SetPowerBICapacityEncryptionKey(encryptionKey.Id, CapacityId);
            }
        }
    }
}
