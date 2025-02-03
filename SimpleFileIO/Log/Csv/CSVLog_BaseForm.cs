using CsvHelper;
using System.Globalization;
using SimpleFileIO.Enum;
using SimpleFileIO.Utility;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;

namespace SimpleFileIO.Log.Csv
{
    internal class CSVLog_BaseForm : ICSVLog
    {
        private List<object> _logItems = new List<object>();
        private readonly object _logItemsMutex = new object();
        private PathProperty _logProperties = new PathProperty();
        private readonly object _logPropertiesMutex = new object();
        private Type? _dataType = null; // Check DataType

        public PathProperty PathProperty
        {
            get
            {
                PathProperty result = new PathProperty();
                lock (_logItemsMutex)
                {
                    if (_logProperties.RootDirectory != null)
                        result.RootDirectory = new DirectoryInfo(_logProperties.RootDirectory.FullName);
                    else
                        result.RootDirectory = new DirectoryInfo("./");
                    result.FileName = _logProperties.FileName;
                    result.Extension = _logProperties.Extension;
                }
                return result;
            }
            set
            {
                lock (_logItemsMutex)
                {
                    _logProperties.RootDirectory = new DirectoryInfo(value.RootDirectory.FullName);
                    _logProperties.FileName = value.FileName;
                    _logProperties.Extension = value.Extension;
                }
            }
        }

        public bool IsRecording { get; private set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">When using, contents such as 
        /// arrays cannot be used.Linearization is required.</typeparam>
        /// <param name="logEntry"></param>
        /// <returns></returns>
        public EAddItemErrorCode Add<T>(T logEntry) where T : class
        {
            if (_dataType == null)
                _dataType = logEntry.GetType();
            else if (_dataType != null && _dataType != logEntry.GetType())
                return EAddItemErrorCode.DifferentDataType;

            EAddItemErrorCode result = EAddItemErrorCode.Unknown;
            try
            {
                lock (_logItemsMutex)
                {
                    _logItems.Add(logEntry);
                    result = EAddItemErrorCode.OK;
                }
            }
            catch (Exception)
            {
                result = EAddItemErrorCode.WorkException;
            }
            return result;
        }

        public EWriteItemErrorCode Write()
        {
            if (IsRecording)
                return EWriteItemErrorCode.AlreadyWriting;

            IsRecording = true;

            List<object> tempItems;
            lock (_logItemsMutex)
            {
                tempItems = new List<object>(_logItems);
                _logItems.Clear();
            }

            if (tempItems.Count == 0)
            {
                IsRecording = false;
                return EWriteItemErrorCode.InvalidEmpty;
            }

            EWriteItemErrorCode result = EWriteItemErrorCode.Unknown;
            var tempProperties = PathProperty;
            try
            {
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
                        csv.NextRecord();
                    }

                    foreach (var item in tempItems)
                    {
                        csv.WriteRecord(item);
                        csv.NextRecord();
                    }
                }
                result = EWriteItemErrorCode.OK;
            }
            catch (Exception)
            {
                result = EWriteItemErrorCode.TaskRunWritingException;
            }
            IsRecording = false;
            return result;
        }

        public EClearItemErrorCode Clear()
        {
            EClearItemErrorCode result = EClearItemErrorCode.Unknown;
            try
            {
                lock (_logItemsMutex)
                {
                    _logItems.Clear();
                    result = EClearItemErrorCode.OK;
                }
            }
            catch (Exception)
            {
                result = EClearItemErrorCode.WorkException;
            }
            return result;
        }

        
    }
}
