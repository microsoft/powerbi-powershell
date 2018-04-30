/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Reports;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Report>))]
    public class GetPowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIReport";

        public GetPowerBIReport() : base() { }

        public GetPowerBIReport(IPowerBIClientCmdletInitFactory init) : base(init) { }

        public override void ExecuteCmdlet()
        {
            using (var client = this.CreateClient())
            {
                var reports = client.Reports.GetReports();
                this.Logger.WriteObject(reports, true);
            }
        }
    }
}
