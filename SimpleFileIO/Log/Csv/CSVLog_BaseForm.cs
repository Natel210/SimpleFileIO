using CsvHelper;
using System.Globalization;
using SimpleFileIO.Utility;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using SimpleFileIO.Resources.ErrorMessages;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimpleFileIO.Log.Csv
{
    /// <summary>
    /// Provides a basic implementation of the <see cref="ICSVLog"/> interface for CSV-based logging.
    /// This class offers functionality to manage log entries, write them to a file, and clear them.
    /// It provides default values internally and allows customization through inheritance.
    /// </summary>
    internal class CSVLog_BaseForm : ICSVLog
    {
        /// <summary>
        /// Internal buffer to store log entries before writing to the file.
        /// </summary>
        private List<object> _logItems = new List<object>();

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
        /// This is for checking the previous type.
        /// </summary>
        private Type? _dataType = null; // Check DataType

        /// <summary>
        /// Gets or sets a value indicating whether exceptions should be thrown when an error occurs.
        /// </summary>
        /// <value>
        /// <c>true</c> if exceptions should be thrown; otherwise, <c>false</c>. 
        /// When set to <c>false</c>, errors will be handled internally without throwing exceptions.
        /// </value>
        public bool ThrowExceptionMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the properties of the file, such as 
        /// the root directory, file name, and extension.
        /// </summary>
        /// <value>
        /// A <see cref="Utility.PathProperty"/> object containing the file's path settings.
        /// </value>
        public PathProperty PathProperty
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
        /// Gets a value indicating whether the logging process is currently in progress.
        /// </summary>
        /// <value>
        /// <c>true</c> if logging is in progress; otherwise, <c>false</c>.
        /// </value>
        public bool IsWriting { get; private set; } = false;

        /// <summary>
        /// Adds a content entry to the internal buffer for deferred writing.
        /// </summary>
        /// <param name="content">The content string to be added to the buffer.</param>
        /// <returns>
        /// <c>true</c> if the content entry was successfully added; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs and <see cref="ThrowExceptionMode"/> is set to <c>true</c>.
        /// </exception>
        public bool Add<T>(T content) where T : class
        {
            if (_dataType == null)
                _dataType = content.GetType();
            else if (_dataType != null && _dataType != content.GetType())
            {
                Debug.WriteLine(ErrorMessages.add_different_data_type);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.add_different_data_type);
                return false;
            }
                
            try
            {
                lock (_logItemsMutex)
                    _logItems.Add(content);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ErrorMessages.add_work);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.add_work, ex);
                return false;
            }
        }

        /// <summary>
        /// Writes all buffered content entries to the specified content file.
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
            List<object> tempItems;

            try
            {
                lock (_logItemsMutex)
                {
                    tempItems = new List<object>(_logItems);
                    _logItems.Clear();
                }

                if (tempItems == null || tempItems.Count == 0)
                {
                    IsWriting = false;
                    throw new InvalidOperationException(ErrorMessages.write_copy_invaild);
                }
            }
            catch (Exception ex)
            {
                IsWriting = false;
                Debug.WriteLine(ErrorMessages.write_deep_copying);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.write_deep_copying, ex);
                return false;
            }

            var tempProperties = PathProperty;
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
                        bool atNewFile = false;
                        if (!fileInfo.Exists)
                            atNewFile = true;
                        using (var writer = new StreamWriter(fileInfo.FullName, append: true))
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            if (atNewFile)
                            {
                                csv.WriteHeader(tempItems.First().GetType());
                                await csv.NextRecordAsync();
                            }

                            foreach (var item in tempItems)
                            {
                                csv.WriteRecord(item);
                                await csv.NextRecordAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        IsWriting = false;
                        Debug.WriteLine(ErrorMessages.write_task_work);
                        if (ThrowExceptionMode is true)
                            throw new InvalidOperationException(ErrorMessages.write_task_work, ex);
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
                    throw new InvalidOperationException(ErrorMessages.write_task_cancellation_token, ex);
                return false;
            }
            IsWriting = false;
            return !isError;
        }

        /// <summary>
        /// Clears all buffered content entries from memory.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the content buffer was successfully cleared; otherwise, <c>false</c>.
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
            catch (Exception ex)
            {
                Debug.WriteLine(ErrorMessages.clear_work);
                if (ThrowExceptionMode is true)
                    throw new Exception(ErrorMessages.clear_work, ex);
                return false;
            }
            return true;
        }

        
    }
}
