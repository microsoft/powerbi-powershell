using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Table
    {
        public string Name { get; set; }
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<Row> Rows { get; set; }
        public IEnumerable<Measure> Measures { get; set; }

        public static implicit operator Table(PowerBI.Api.V2.Models.Table table)
        {
            return new Table
            {
                Name = table.Name,
                // Columns
                // Rows
                // Measures
            };
        }
    }
}
