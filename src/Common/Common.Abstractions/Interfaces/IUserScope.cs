using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    public interface IUserScope
    {
        PowerBIUserScope Scope { get; set; }
    }
}
