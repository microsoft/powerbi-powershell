/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    [DataContract]
    public class PowerBIConfiguration
    {
        [DataMember(Name = "Environments")]
        public IEnumerable<PowerBIConfigurationEnvironment> Environments { get; set; }

        [DataMember(Name = "Settings")]
        public PowerBIConfigurationSettings Settings { get; set; }
    }

    [DataContract]
    public class PowerBIConfigurationEnvironment
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "cloudName", IsRequired = false)]
        public string CloudName { get; set; }

        [DataMember(Name = "authority", IsRequired = false)]
        public string Authority { get; set; }

        [DataMember(Name = "clientId", IsRequired = false)]
        public string ClientId { get; set; }

        [DataMember(Name = "redirect", IsRequired = false)]
        public string Redirect { get; set; }

        [DataMember(Name = "resource", IsRequired = false)]
        public string Resource { get; set; }

        [DataMember(Name = "globalService", IsRequired = false)]
        public string GlobalService { get; set; }
    }

    [DataContract]
    public class PowerBIConfigurationSettings : IPowerBIConfigurationSettings
    {
        [DataMember(Name = "ForceDeviceCodeAuthentication", IsRequired = false)]
        public bool ForceDeviceCodeAuthentication { get; set; }

        [DataMember(Name = "ShowMSALDebugMessages", IsRequired = false)]
        public bool ShowMSALDebugMessages { get; set; }

        [DataMember(Name = "DefaultClientId", IsRequired = false)]
        public string DefaultClientId { get; set; }

        [DataMember(Name = "DefaultRedirect", IsRequired = false)]
        public string DefaultRedirect { get; set; }
    }
}
