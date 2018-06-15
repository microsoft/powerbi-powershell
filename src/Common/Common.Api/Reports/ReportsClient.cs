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

        public IEnumerable<Report> GetReports(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Reports.GetReports(filter: filter, top: top, skip: skip).Value.Select(x => (Report)x);
        }

        public IEnumerable<Report> GetReportsAsAdmin(string filter = null, int? top = null, int? skip = null)
        {
            return this.Client.Reports.GetReportsAsAdmin(filter: filter, top: top, skip: skip).Value.Select(x => (Report)x);
        }
    }
}
