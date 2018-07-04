/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }

        public static implicit operator Column(PowerBI.Api.V2.Models.Column column)
        {
            if(column == null)
            {
                return null;
            }

            return new Column
            {
                Name = column.Name,
                DataType = column.DataType
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Column(Column column)
        {
            if (column == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Column
            {
                Name = column.Name,
                DataType = column.DataType
            };
        }
    }
}
