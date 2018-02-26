using System;
using System.Management.Automation;

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// PowerBI Logger to write output, host (console), debug, verbose, warning, and error messages to PowerShell streams.
    /// </summary>
    public interface IPowerBILogger
    {
        /// <summary>
        /// Cmdlet for the logger to write to.
        /// </summary>
        PSCmdlet Cmdlet { get; set; }

        /// <summary>
        /// Write error to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value of the error to write to the error stream.</param>
        /// <param name="category">Category to log the error against; default is WriteError.</param>
        void WriteError(object obj, ErrorCategory category = ErrorCategory.WriteError);

        /// <summary>
        /// Write error to the PowerBI logger.
        /// </summary>
        /// <param name="ex">Exception to write to the error stream.</param>
        /// <param name="category">Category to log the error against; default is WriteError.</param>
        void WriteError(Exception ex, ErrorCategory category = ErrorCategory.WriteError);

        /// <summary>
        /// Write error to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value of the error to the error stream.</param>
        /// <param name="ex">Exception to log with the provided value to the error stream.</param>
        /// <param name="category">Category to log the error against; default is WriteError.</param>
        void WriteError(object obj, Exception ex, ErrorCategory category = ErrorCategory.WriteError);

        /// <summary>
        /// Write error to the PowerBI logger in the form of a ErrorRecord.
        /// </summary>
        /// <param name="record">Error record to write to the error stream.</param>
        void WriteError(ErrorRecord record);

        /// <summary>
        /// Write a warning message to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value to write to the warning stream.</param>
        void WriteWarning(object obj);

        /// <summary>
        /// Write a verbose message to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value of message to write to the verbose stream.</param>
        void WriteVerbose(object obj);

        /// <summary>
        /// Write a debug message to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value of the message to write to debug stream.</param>
        void WriteDebug(object obj);

        /// <summary>
        /// Write an output object to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value to write to output stream.</param>
        /// <param name="enumerateCollection">Indicates to enumerate a collection when writing to output stream; default is null which picks PowerShell's default for enumeration.</param>
        void WriteObject(object obj, bool? enumerateCollection = null);

        /// <summary>
        /// Write a host message to the PowerBI logger.
        /// </summary>
        /// <param name="obj">Value of the message.</param>
        /// <param name="foregroundColor">Foreground color to write the host message with; default is null which causes the logger to write in the host's default foreground color.</param>
        /// <param name="backgroundColor">Background color to write the host message with; default is null which causes the logger to write in the host's default background color.</param>
        void WriteHost(object obj, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null);

        /// <summary>
        /// Flush any messages that couldn't be sent due to operating on non-main threads.
        /// </summary>
        void FlushMessages();
    }
}