/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public class Report
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WebUrl { get; set; }

        public string EmbedUrl { get; set; }

        public string DatasetId { get; set; }

        public static implicit operator Report(PowerBI.Api.V2.Models.Report report)
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
