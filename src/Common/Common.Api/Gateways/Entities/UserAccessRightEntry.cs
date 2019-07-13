/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Runtime.Serialization;
using Microsoft.PowerBI.Api.V2.Models;

namespace Microsoft.PowerBI.Common.Api.Gateways.Entities
{
    [DataContract]
    public class UserAccessRightEntry
    {
        [DataMember(Name = "datasourceAccessRight")]
        public DatasourceUserAccessRight? DatasourceUserAccessRight { get; set; }

        [DataMember(Name = "groupUserAccessRight")]
        public GroupUserAccessRight GroupUserAccessRight { get; set; }

        [DataMember(Name = "emailAddress")]
        public string UserEmailAddress { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "principalType")]
        public PrincipalType? PrincipalType { get; set; }
    }
}