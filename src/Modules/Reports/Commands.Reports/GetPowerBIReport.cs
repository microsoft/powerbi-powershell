/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.PowerBI.Commands.Common;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Reports
{
    [Cmdlet(CmdletVerb, CmdletName)]
    [OutputType(typeof(IEnumerable<Report>))]
    public class GetPowerBIReport : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.Get;
        public const string CmdletName = "PowerBIReport";

        public override void ExecuteCmdlet()
        {
            IPowerBIClient client = this.CreateClient();

            var reports = client.Reports.GetReports();
            this.Logger.WriteObject(reports.Value, true);
        }
    }
}
