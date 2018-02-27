using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Base Factory for Authentication (common among authentication factories).
    /// </summary>
    public interface IAuthenticationBaseFactory
    {
        /// <summary>
        /// Indicates the factory has been used once to authenticate; otherwise it is the first use of the factory.
        /// </summary>
        bool AuthenticatedOnce { get; }

        /// <summary>
        /// Revokes any previous authentication session.
        /// </summary>
        void Challenge();
    }
}
