using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Row
    {
        public Guid Id { get; set; }

        public static implicit operator Row(PowerBI.Api.V2.Models.Row row)
        {
            return new Row
            {
                Id = new Guid(row.Id)
            };
        }
    }
}
