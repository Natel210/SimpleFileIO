using System.IO;

namespace SimpleFileIO.Utility
{
    public struct PathProperty
    {
        public DirectoryInfo RootDirectory;
        public string FileName;
        /// <summary>
        /// not include '.'
        /// </summary>
        public string Extension;
    }
}
