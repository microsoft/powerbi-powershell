using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Row
    {


        public static implicit operator Row(PowerBI.Api.V2.Models.Row row)
        {
            return new Row
            {
                
            };
        }
    }
}
