/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Api.Dataflows
{
    public class Dataflow
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ModelUrl { get; set; }
        public string ConfiguredBy { get; set; }

        public static implicit operator Dataflow(PowerBI.Api.V2.Models.Dataflow dataflow)
        {
            if (dataflow == null)
            {
                return null;
            }

            return new Dataflow
            {
                Id = dataflow.ObjectId,
                Name = dataflow.Name,
                Description = dataflow.Description,
                ModelUrl = dataflow.ModelUrl,
                ConfiguredBy = dataflow.ConfiguredBy
            };
        }
    }
}
 