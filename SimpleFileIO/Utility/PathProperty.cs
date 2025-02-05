using System.IO;

namespace SimpleFileIO.Utility
{
    /// <summary>
    /// Represents a file path configuration used for managing log and state file locations.
    /// This structure defines the root directory, file name, and extension for a file.
    /// </summary>
    public struct PathProperty
    {
        /// <summary>
        /// Gets or sets the root directory where the file will be stored.
        /// This should be a valid directory path.
        /// </summary>
        public DirectoryInfo RootDirectory;

        /// <summary>
        /// Gets or sets the name of the file without an extension.
        /// Ensure that this does not include invalid file system characters.
        /// </summary>
        public string FileName;

        /// <summary>
        /// Gets or sets the file extension.
        /// Do not include the dot ('.') in this value (e.g., use "txt" instead of ".txt").
        /// </summary>
        public string Extension;
    }
}
