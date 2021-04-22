/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Workbooks
{
    public class Workbook
    {
        public string Name { get; set; }

        public string DatasetId { get; set; }
        
        public static implicit operator Workbook(PowerBI.Api.V2.Models.Workbook workbook)
        {
            if (workbook == null)
            {
                return null;
            }

            return new Workbook()
            {
                Name = workbook.Name,
                DatasetId = workbook.DatasetId
            };
        }
    }
}
