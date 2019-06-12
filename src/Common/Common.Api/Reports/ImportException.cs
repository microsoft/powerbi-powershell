/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

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
