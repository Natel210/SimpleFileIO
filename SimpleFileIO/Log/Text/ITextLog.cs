using SimpleFileIO.Enum;
using SimpleFileIO.Utility;

namespace SimpleFileIO.Log.Text
{
    public interface ITextLog
    {
        /// <summary>
        /// Logger properties.
        /// </summary>
        PathProperty Properties { get; set; }

        /// <summary>
        /// Indicates if writing is enabled.
        /// </summary>
        bool IsWriting { get; }

        /// <summary>
        /// Adds a log entry to the buffer.
        /// </summary>
        /// <param name="log">Log string to write</param>
        EAddItemErrorCode Add(string log);

        /// <summary>
        /// Writes the buffered log entries.
        /// </summary>
        EWriteItemErrorCode Write();

        /// <summary>
        /// Clears the buffered log entries.
        /// </summary>
        EClearItemErrorCode Clear();
    }
}
