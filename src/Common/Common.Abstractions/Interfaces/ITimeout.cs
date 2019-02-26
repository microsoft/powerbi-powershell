using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface ITimeout
    {
        TimeSpan? Timeout { get; set; }
    }
}
