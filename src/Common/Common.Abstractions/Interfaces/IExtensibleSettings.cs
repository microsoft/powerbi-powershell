using System.Collections.Generic;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Extensible settings.
    /// </summary>
    public interface IExtensibleSettings
    {
        /// <summary>
        /// Dictionary to store settings.
        /// </summary>
        IDictionary<string, string> Settings { get; }
    }
}