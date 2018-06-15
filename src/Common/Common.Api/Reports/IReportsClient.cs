/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public interface IReportsClient
    {
        IEnumerable<Report> GetReports(string filter = default, int? top = default, int? skip = default);

        IEnumerable<Report> GetReportsAsAdmin(string filter = default, int? top = default, int? skip = default);
    }
}
