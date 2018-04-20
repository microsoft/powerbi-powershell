/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public class ReportsClient : PowerBIEntityClient, IReportsClient
    {
        public ReportsClient(IPowerBIClient client) : base(client)
        {
        }

        public IEnumerable<Report> GetReports()
        {
            return this.Client.Reports.GetReports().Value.Select(x => ReportsConversion.ToReport(x));
        }
    }
}
