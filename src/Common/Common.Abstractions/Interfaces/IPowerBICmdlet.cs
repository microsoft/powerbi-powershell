namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// PowerBI PowerShell Cmdlet.
    /// </summary>
    public interface IPowerBICmdlet
    {
        /// <summary>
        /// Main thread ID of the executing cmdlet.
        /// </summary>
        int MainThreadId { get; }
    }
}