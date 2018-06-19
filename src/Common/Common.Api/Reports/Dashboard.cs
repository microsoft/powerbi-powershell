using System;
using System.Collections.Generic;
using System.Text;

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
                Id = new Guid(dashboard.Id),
                Name = dashboard.DisplayName,
                IsReadOnly = dashboard.IsReadOnly,
                EmbedUrl = dashboard.EmbedUrl
            };
        }
    }
}
