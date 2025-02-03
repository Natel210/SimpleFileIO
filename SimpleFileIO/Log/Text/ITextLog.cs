using SimpleFileIO.Enum;
using SimpleFileIO.Utility;
using System;

namespace SimpleFileIO.Log.Text
{
    /// <summary>
    /// Defines an interface for text-based logging operations.
    /// This interface provides functionality to manage log entries, write them to a file, and clear them.
    /// It is designed to be extendable to allow for specialized behavior through inheritance.
    /// </summary>
    public interface ITextLog
    {
        /// <summary>
        /// Gets or sets the properties of the log file, such as 
        /// the root directory, file name, and extension.
        /// </summary>
        /// <value>
        /// A <see cref="PathProperty"/> object containing the log file's path settings.
        /// </value>
        PathProperty Properties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether exceptions should be thrown when an error occurs.
        /// </summary>
        /// <value>
        /// <c>true</c> if exceptions should be thrown; otherwise, <c>false</c>. 
        /// When set to <c>false</c>, errors will be handled internally without throwing exceptions.
        /// </value>
        bool ThrowExceptionMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the logging process is currently in progress.
        /// </summary>
        /// <value>
        /// <c>true</c> if logging is in progress; otherwise, <c>false</c>.
        /// </value>
        bool IsWriting { get; }

        /// <summary>
        /// Adds a log entry to the internal buffer for deferred writing.
        /// </summary>
        /// <param name="log">The log string to be added to the buffer.</param>
        /// <returns>
        /// <c>true</c> if the log entry was successfully added; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs and <see cref="ThrowExceptionMode"/> is set to <c>true</c>.
        /// </exception>
        bool Add(string log);

        /// <summary>
        /// Writes all buffered log entries to the specified log file.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the write operation was successful; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if logging is already in progress and <see cref="ThrowExceptionMode"/> is set to <c>true</c>.
        /// </exception>
        bool Write();

        /// <summary>
        /// Clears all buffered log entries from memory.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the log buffer was successfully cleared; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an error occurs during the clearing process and <see cref="ThrowExceptionMode"/> is set to <c>true</c>.
        /// </exception>
        bool Clear();
    }
}
