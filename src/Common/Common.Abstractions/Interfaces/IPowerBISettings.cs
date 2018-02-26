using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// PowerBI default settings.
    /// </summary>
    public interface IPowerBISettings : IExtensibleSettings
    {
        /// <summary>
        /// Available PowerBI environment settings to pick from.
        /// </summary>
        IDictionary<PowerBIEnvironmentType, IPowerBIEnvironment> Environments { get; }
    }
}