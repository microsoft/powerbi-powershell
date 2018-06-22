/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Measure
    {
        public string Name { get; set; }
        public string Expression { get; set; }

        public static implicit operator Measure(PowerBI.Api.V2.Models.Measure measure)
        {
            return new Measure
            {
                Name = measure.Name,
                Expression = measure.Expression
            };
        }
    }
}
