/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Api.Reports
{
    public class Tile
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int? RowSpan { get; set; }
        public int? ColumnSpan { get; set; }
        public string EmbedUrl { get; set; }
        public string EmbedData { get; set; }
        public Guid? ReportId { get; set; }
        public Guid? DatasetId { get; set; }

        public static implicit operator Tile(PowerBI.Api.V2.Models.Tile tile)
        {
            return new Tile()
            {
                Id = tile.Id,
                Title = tile.Title,
                RowSpan = tile.RowSpan,
                ColumnSpan = tile.ColSpan,
                EmbedUrl = tile.EmbedUrl,
                EmbedData = tile.EmbedData,
                ReportId = tile.ReportId,
                DatasetId = tile.DatasetId
            };
        }
    }
}
