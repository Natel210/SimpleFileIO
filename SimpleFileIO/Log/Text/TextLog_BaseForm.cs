using SimpleFileIO.Enum;
using SimpleFileIO.Resources.ErrorMessages;
using SimpleFileIO.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SimpleFileIO.Log.Text
{
    /// <summary>
    /// Provides a basic implementation of the <see cref="ITextLog"/> interface for text-based logging.
    /// This class offers functionality to manage log entries, write them to a file, and clear them.
    /// It provides default values internally and allows customization through inheritance.
    /// </summary>
    internal class TextLog_BaseForm : ITextLog
    {
        /// <summary>
        /// Internal buffer to store log entries before writing to the file.
        /// </summary>
        private List<string> _logItems = new List<string>();

        /// <summary>
        /// Mutex object to ensure thread safety for log entry operations.
        /// </summary>
        private readonly object _logItemsMutex = new object();

        /// <summary>
        /// Stores the properties of the log file, such as directory, file name, and extension.
        /// </summary>
        private PathProperty _logProperties = new PathProperty();

        /// <summary>
        /// Mutex object to ensure thread safety for log property operations.
        /// </summary>
        private readonly object _logPropertiesMutex = new object();

        /// <summary>
        /// Gets or sets the properties of the log file, such as 
        /// the root directory, file name, and extension.
        /// </summary>
        /// <value>
        /// A <see cref="PathProperty"/> object containing the log file's path settings.
        /// </value>
        public PathProperty Properties
        {
            get
            {
                PathProperty result = new PathProperty();
                try
                {
                    lock (_logPropertiesMutex)
                    {
                        result = new PathProperty();
                        if (_logProperties.RootDirectory is not null)
                            result.RootDirectory = new DirectoryInfo(_logProperties.RootDirectory.FullName);
                        result.FileName = _logProperties.FileName;
                        result.Extension = _logProperties.Extension;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ErrorMessages.path_property_work);
                    if (ThrowExceptionMode is true)
                        throw new Exception(ErrorMessages.path_property_work, ex);
                }
                return result;
            }
            set
            {
                try
                {
                    lock (_logPropertiesMutex)
                    {
                        _logProperties.RootDirectory = new DirectoryInfo(value.RootDirectory.FullName);
                        _logProperties.FileName = value.FileName;
                        _logProperties.Extension = value.Extension;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ErrorMessages.path_property_work);
                    if (ThrowExceptionMode is true)
                        throw new Exception(ErrorMessages.path_property_work, ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether exceptions should be thrown when an error occurs.
        /// </summary>
        /// <value>
        /// <c>true</c> if exceptions should be thrown; otherwise, <c>false</c>. 
        /// When set to <c>false</c>, errors will be handled internally without throwing exceptions.
        /// </value>
        public bool ThrowExceptionMode { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether the logging process is currently in progress.
        /// </summary>
        /// <value>
        /// <c>true</c> if logging is in progress; otherwise, <c>false</c>.
        /// </value>
        public bool IsWriting { get; private set; } = false;

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
        public bool Add(string log)
        {
            try
            {
                lock (_logItemsMutex)
                    _logItems.Add(log);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ErrorMessages.add_work);
                if (ThrowExceptionMode is true)
                    throw new Exception(ErrorMessages.add_work, ex);
                return false;
            }
        }

        /// <summary>
        /// Writes all buffered log entries to the specified log file.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the write operation was successful; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if logging is already in progress and <see cref="ThrowExceptionMode"/> is set to <c>true</c>.
        /// </exception>
        public bool Write()
        {
            if (IsWriting)
            {
                Debug.WriteLine(ErrorMessages.write_already_writing);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.write_already_writing);
                return false;
            }

            IsWriting = true;
            List<string>? tempItems = null;

            try
            {
                lock (_logItemsMutex)
                {
                    if (_logItems.Count != 0)
                    {
                        tempItems = new List<string>(_logItems);
                        _logItems.Clear();
                    }

                    if (tempItems == null || tempItems.Count == 0)
                    {
                        Debug.WriteLine(ErrorMessages.write_copy_invaild);
                        throw new InvalidOperationException(ErrorMessages.write_copy_invaild);
                    }
                }
            }
            catch (Exception ex)
            {
                IsWriting = false;
                Debug.WriteLine(ErrorMessages.write_deep_copying);
                if (ThrowExceptionMode is true)
                    throw new Exception(ErrorMessages.write_deep_copying, ex);
                return false;
            }

            var tempProperties = Properties;
            bool isError = false;
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        if (tempProperties.RootDirectory is null)
                        {
                            Debug.WriteLine(ErrorMessages.write_task_root_directory_not_found);
                            throw new DirectoryNotFoundException(ErrorMessages.write_task_root_directory_not_found);
                        }
                            
                        if (!tempProperties.RootDirectory.Exists)
                            tempProperties.RootDirectory.Create();
                        FileInfo fileInfo = new FileInfo(Path.Combine(tempProperties.RootDirectory.FullName, $"{tempProperties.FileName}.{tempProperties.Extension}"));
                        using var fileStream = new FileStream(fileInfo.FullName, fileInfo.Exists ? FileMode.Append : FileMode.Create);
                        using var streamWriter = new StreamWriter(fileStream);
                        foreach (var item in tempItems)
                            await streamWriter.WriteLineAsync(item);
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        IsWriting = false;
                        Debug.WriteLine(ErrorMessages.write_task_work);
                        if (ThrowExceptionMode is true)
                            throw new Exception(ErrorMessages.write_task_work, ex);
                        return;
                    }
                }).Wait();
            }
            catch (Exception ex)
            {
                isError = true;
                IsWriting = false;
                Debug.WriteLine(ErrorMessages.write_task_cancellation_token);
                if (ThrowExceptionMode is true)
                    throw new Exception(ErrorMessages.write_task_cancellation_token, ex);
                return false;
            }
            IsWriting = false;
            return !isError;
        }

        /// <summary>
        /// Clears all buffered log entries from memory.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the log buffer was successfully cleared; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an error occurs during the clearing process and <see cref="ThrowExceptionMode"/> is set to <c>true</c>.
        /// </exception>
        public bool Clear()
        {
            try
            {
                lock (_logItemsMutex)
                    _logItems.Clear();
            }
            catch (Exception)
            {
                Debug.WriteLine("WorkException");
                if (ThrowExceptionMode)
                    throw new InvalidOperationException("WorkException");
                return false;
            }
            return true;
        }
    }
}
