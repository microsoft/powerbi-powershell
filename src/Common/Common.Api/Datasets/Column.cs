using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Column
    {

        public static implicit operator Column(PowerBI.Api.V2.Models.Column row)
        {
            return new Column
            {

            };
        }
    }
}
