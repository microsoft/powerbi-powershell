namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Profile containing information for an authenticated user\principal.
    /// </summary>
    public interface IPowerBIProfile
    {
        /// <summary>
        /// PowerBI environment to authenticate and connumicate against.
        /// </summary>
        IPowerBIEnvironment Environment { get; }

        /// <summary>
        /// Tenant ID for authenticated user\principal.
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// Displayable name of user\prinicpal authenticated against.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Type of login used to create profile.
        /// </summary>
        PowerBIProfileType LoginType { get; }
    }
}