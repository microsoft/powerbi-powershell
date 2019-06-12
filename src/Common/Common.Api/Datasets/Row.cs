/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Row
    {
        public Guid Id { get; set; }

        public static implicit operator Row(PowerBI.Api.V2.Models.Row row)
        {
            if(row == null)
            {
                return null;
            }

            return new Row
            {
                Id = new Guid(row.Id)
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Row(Row row)
        {
            if (row == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Row
            {
                Id = row.Id.ToString()
            };
        }
    }
}
