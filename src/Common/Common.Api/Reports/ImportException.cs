using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    class ImportException : Exception
    {
        public ImportException(Guid importId, string reportName, string importState) : base(string.Format(
                    "The import was not successful for import ID '{0}', report name '{1}'. The import state was '{2}'.",
                    importId,
                    reportName,
                    importState
                ))
        { }
    }
}
