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
    }
}
