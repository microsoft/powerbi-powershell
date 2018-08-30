using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Imports
{
    public interface IImportsClient
    {
        Guid PostImport(string datasetDisplayName, string filePath);
    }
}
