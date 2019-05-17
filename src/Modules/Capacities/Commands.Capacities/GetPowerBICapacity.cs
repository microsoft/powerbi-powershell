/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Api.Capacities;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Capacities
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Capacity>))]
    public class GetPowerBICapacity : PowerBIClientCmdlet
    {
        public const string CmdletName = "PowerBICapacity";
        public const string CmdletVerb = VerbsCommon.Get;
        private const string ExpandVariable = "GetPowerBICapacityExpandVariable";
        private const string AdminVariable = "GetPowerBICapacityAdminVariable";

        public GetPowerBICapacity() : base() { }

        public GetPowerBICapacity(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false)]
        public PowerBIGetCapacityExpandEnum Show { get; set; } = PowerBIGetCapacityExpandEnum.None;

        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (this.Scope == PowerBIUserScope.Individual && this.Show == PowerBIGetCapacityExpandEnum.EncryptionKey)
            {
                this.Logger.ThrowTerminatingError(
                    string.Format("{0} on {1} is only applied when -{2} is set to {3}",
                    nameof(this.Show),
                    Enum.GetName(typeof(PowerBIGetCapacityExpandEnum), this.Show),
                    nameof(this.Scope),
                    nameof(PowerBIUserScope.Organization)));
            }
        }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                IEnumerable<Capacity> capacities = null;
                SessionState?.PSVariable?.Set(AdminVariable, this.Scope == PowerBIUserScope.Organization);

                if (this.Scope == PowerBIUserScope.Individual)
                {
                    SessionState?.PSVariable?.Set(ExpandVariable, false);
                    capacities = client.Capacities.GetCapacities();
                }
                else
                {
                    var expand = this.Show == PowerBIGetCapacityExpandEnum.EncryptionKey ? "tenantKey" : null;
                    SessionState?.PSVariable?.Set(ExpandVariable, this.Show == PowerBIGetCapacityExpandEnum.EncryptionKey);
                    capacities = client.Capacities.GetCapacitiesAsAdmin(expand);
                }

                this.Logger.WriteObject(capacities, enumerateCollection: true);
            }
        }
    }

    public enum PowerBIGetCapacityExpandEnum
    {
        None = 0,
        EncryptionKey = 1
    }
}
