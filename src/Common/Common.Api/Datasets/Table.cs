/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;

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
            if(table == null)
            {
                return null;
            }

            return new Table
            {
                Name = table.Name,
                Columns = table.Columns?.Select(c => (Column)c),
                Rows = table.Rows?.Select(r => (Row)r),
                Measures = table.Measures?.Select(m => (Measure)m)
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Table(Table table)
        {
            if (table == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Table
            {
                Name = table.Name,
                Columns = table.Columns?.Select(c => (PowerBI.Api.V2.Models.Column)c).ToList(),
                Rows = table.Rows?.Select(r => (PowerBI.Api.V2.Models.Row)r).ToList(),
                Measures = table.Measures?.Select(m => (PowerBI.Api.V2.Models.Measure)m).ToList()
            };
        }
    }
}
