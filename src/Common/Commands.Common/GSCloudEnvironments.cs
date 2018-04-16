/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.PowerBI.Commands.Common
{
    [DataContract]
    public class GSEnvironments
    {
        [DataMember(Name = "environments")]
        public IEnumerable<GSEnvironment> Environments { get; set; }
    }

    [DataContract]
    public class GSEnvironment
    {
        [DataMember(Name = "cloudName")]
        public string CloudName { get; set; }

        [DataMember(Name = "services")]
        public IEnumerable<GSEnvironmentService> Services { get; set; }
    }

    [DataContract]
    public class GSEnvironmentService
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "endpoint")]
        public string Endpoint { get; set; }
        [DataMember(Name = "resourceId")]
        public string ResourceId { get; set; }
    }
}
