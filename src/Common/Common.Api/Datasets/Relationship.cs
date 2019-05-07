/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using Microsoft.PowerBI.Common.Api.Helpers;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Relationship
    {
        public string Name { get; set; }
        public CrossFilteringBehaviorEnum? CrossFilteringBehavior { get; set; }
        public string FromTable { get; set; }
        public string FromColumn { get; set; }
        public string ToTable { get; set; }
        public string ToColumn { get; set; }

        public static implicit operator Relationship(PowerBI.Api.V2.Models.Relationship relationship)
        {
            if(relationship == null)
            {
                return null;
            }

            return new Relationship
            {
                Name = relationship.Name,
                CrossFilteringBehavior = EnumTypeConverter.ConvertTo<CrossFilteringBehaviorEnum, PowerBI.Api.V2.Models.CrossFilteringBehavior>(relationship.CrossFilteringBehavior),
                FromTable = relationship.FromTable,
                FromColumn = relationship.FromColumn,
                ToTable = relationship.ToTable,
                ToColumn = relationship.ToColumn
            };
        }

        public static implicit operator PowerBI.Api.V2.Models.Relationship(Relationship relationship)
        {
            if (relationship == null)
            {
                return null;
            }

            return new PowerBI.Api.V2.Models.Relationship
            {
                Name = relationship.Name,
                CrossFilteringBehavior = EnumTypeConverter.ConvertTo<PowerBI.Api.V2.Models.CrossFilteringBehavior, CrossFilteringBehaviorEnum>(relationship.CrossFilteringBehavior),
                FromTable = relationship.FromTable,
                FromColumn = relationship.FromColumn,
                ToTable = relationship.ToTable,
                ToColumn = relationship.ToColumn
            };
        }
    }

    public enum CrossFilteringBehaviorEnum
    {
        OneDirection,
        BothDirections,
        Automatic
    }
}
