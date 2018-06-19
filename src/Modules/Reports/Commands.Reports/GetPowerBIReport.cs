/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Abstractions;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = ListParameterSetName)]
    [OutputType(typeof(IEnumerable<Report>))]
    public class GetPowerBIReport : PowerBIClientCmdlet, IUserScope, IUserFilter, IUserId
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIReport";

        private const string IdParameterSetName = "Id";
        private const string NameParameterSetName = "Name";
        private const string ListParameterSetName = "List";

        public GetPowerBIReport() : base() { }

        public GetPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }

        #region Parameters
        [Parameter(Mandatory = true, ParameterSetName = IdParameterSetName)]
        [Alias("ReportId")]
        public Guid Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NameParameterSetName)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public PowerBIUserScope Scope { get; set; } = PowerBIUserScope.Individual;

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public string Filter { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        [Alias("Top")]
        public int? First { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ListParameterSetName)]
        public int? Skip { get; set; }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if(this.Scope == PowerBIUserScope.Individual && !string.IsNullOrEmpty(this.Filter))
            {
                this.Logger.ThrowTerminatingError($"{nameof(this.Filter)} is only applied when -{nameof(this.Scope)} is set to {nameof(PowerBIUserScope.Organization)}");
            }
        }

        public override void ExecuteCmdlet()
        {
            if (this.ParameterSet.Equals(IdParameterSetName))
            {
                this.Filter = $"id eq '{this.Id}'";
            }

            if (this.ParameterSet.Equals(NameParameterSetName))
            {
                this.Filter = $"tolower(name) eq '{this.Name.ToLower()}'";
            }

            IEnumerable<Report> reports = null;
            using (var client = this.CreateClient())
            {
                reports = this.Scope == PowerBIUserScope.Individual ?
                    client.Reports.GetReports() :
                    client.Reports.GetReportsAsAdmin(filter: this.Filter, top: this.First, skip: this.Skip);
            }

            if(this.Scope == PowerBIUserScope.Individual)
            {
                if(this.Id != default)
                {
                    reports = reports.Where(r => this.Id == new Guid(r.Id)).ToList();
                }

                if(!string.IsNullOrEmpty(this.Name))
                {
                    reports.Where(r => r.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if(this.Skip.HasValue)
                {
                    reports = reports.Skip(this.Skip.Value);
                }

                if(this.First.HasValue)
                {
                    reports = reports.Take(this.First.Value);
                }
            }

            this.Logger.WriteObject(reports, true);
        }
    }
}
