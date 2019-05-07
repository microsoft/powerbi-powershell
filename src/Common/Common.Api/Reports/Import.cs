/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Common.Api.Datasets;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public class Import
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImportState { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<Dataset> Datasets { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public static implicit operator Import(PowerBI.Api.V2.Models.Import import)
        {
            return new Import
            {
                Id = import.Id,
                Name = import.Name,
                ImportState = import.ImportState,
                Reports = import.Reports?.Select(x => (Report)x),
                Datasets = import.Datasets?.Select(x => (Dataset)x),
                Created = import.CreatedDateTime,
                Updated = import.UpdatedDateTime
            };
        }
    }
}
