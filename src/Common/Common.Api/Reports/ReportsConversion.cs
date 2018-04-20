/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public static class ReportsConversion
    {
        public static Report ToReport(PowerBI.Api.V2.Models.Report report)
        {
            return new Report
            {
                Id = report.Id,
                Name = report.Name,
                WebUrl = report.WebUrl,
                EmbedUrl = report.EmbedUrl,
                DatasetId = report.DatasetId,
            };
        }
    }
}
