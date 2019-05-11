/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public class Dashboard
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? IsReadOnly { get; set; }
        public string EmbedUrl { get; set; }

        public static implicit operator Dashboard(PowerBI.Api.V2.Models.Dashboard dashboard)
        {
            return new Dashboard()
            {
                Id = dashboard.Id,
                Name = dashboard.DisplayName,
                IsReadOnly = dashboard.IsReadOnly,
                EmbedUrl = dashboard.EmbedUrl
            };
        }
    }
}
