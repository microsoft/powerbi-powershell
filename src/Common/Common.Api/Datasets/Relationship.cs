/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Api.Datasets
{
    public class Relationship
    {
        public string Name { get; set; }
        public CrossFilteringBehaviorEnum CrossFilteringBehavior { get; set; }
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
                CrossFilteringBehavior = ConvertCrossFilteringBehavior(relationship.CrossFilteringBehavior),
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
                CrossFilteringBehavior = (PowerBI.Api.V2.Models.CrossFilteringBehaviorEnum)Enum.Parse(typeof(PowerBI.Api.V2.Models.CrossFilteringBehaviorEnum), relationship.CrossFilteringBehavior.ToString(), true),
                FromTable = relationship.FromTable,
                FromColumn = relationship.FromColumn,
                ToTable = relationship.ToTable,
                ToColumn = relationship.ToColumn
            };
        }

        private static CrossFilteringBehaviorEnum ConvertCrossFilteringBehavior(PowerBI.Api.V2.Models.CrossFilteringBehaviorEnum? crossFilteringBehavior)
        {
            if(crossFilteringBehavior == null)
            {
                return CrossFilteringBehaviorEnum.NotAvailable;
            }

            return (CrossFilteringBehaviorEnum)Enum.Parse(typeof(CrossFilteringBehaviorEnum), crossFilteringBehavior.Value.ToString(), true);
        }
    }

    public enum CrossFilteringBehaviorEnum
    {
        OneDirection,
        BothDirections,
        Automatic,
        NotAvailable
    }
}
