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
            if(measure == null)
            {
                return null;
            }

            return new Measure
            {
                Name = measure.Name,
                Expression = measure.Expression
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Measure(Measure measure)
        {
            if (measure == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Measure
            {
                Name = measure.Name,
                Expression = measure.Expression
            };
        }
    }
}
