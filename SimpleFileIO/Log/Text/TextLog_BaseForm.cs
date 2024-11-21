using SimpleFileIO.Enum;
using SimpleFileIO.Utility;

namespace SimpleFileIO.Log.Text
{
    internal class TextLog_BaseForm : ITextLog
    {

        private List<string> _logItems = new List<string>();
        private readonly object _logItemsMutex = new object();
        private PathProperty _logProperties = new PathProperty();
        private readonly object _logPropertiesMutex = new object();

        /// <summary>
        /// Logger properties.
        /// </summary>
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
                catch (Exception)
                {
#if DEBUG
                    throw;
#endif
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
                catch (Exception)
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }

        /// <summary>
        /// Indicates if writing is enabled.
        /// </summary>
        public bool IsWriting { get; private set; } = false;

        /// <summary>
        /// Adds a log entry to the buffer.
        /// </summary>
        /// <param name="log">Log string to write</param>
        public EAddItemErrorCode Add(string log)
        {
            EAddItemErrorCode result = EAddItemErrorCode.Unknown;
            try
            {
                lock (_logItemsMutex)
                {
                    _logItems.Add(log);
                    result = EAddItemErrorCode.OK;
                }
            }
            catch (Exception)
            {
                result = EAddItemErrorCode.WorkException;
#if DEBUG
                throw;
#endif
            }
            return result;
        }

        /// <summary>
        /// Writes the buffered log entries.
        /// </summary>
        public EWriteItemErrorCode Write()
        {
            if (IsWriting is true)
                return EWriteItemErrorCode.AlreadyWriting;
            IsWriting = true;
            EWriteItemErrorCode result = EWriteItemErrorCode.Unknown;
            List<string>? tempItems = null;
            try
            {
                lock (_logItemsMutex)
                {
                    if (_logItems.Count is 0)
                        result = EWriteItemErrorCode.OK;
                    else
                    {
                        tempItems = new List<string>(_logItems);
                        _logItems.Clear();
                    }
                }
            }
            catch (Exception)
            {
                result = EWriteItemErrorCode.GetLogItemsException;
                IsWriting = false;
#if DEBUG
                throw;
#endif
            }

            if (result is EWriteItemErrorCode.GetLogItemsException)
            {
                IsWriting = false;
                return result;
            }
            if (result is EWriteItemErrorCode.OK)
            {
                IsWriting = false;
                return result;
            }
            if (tempItems is null)
            {
                IsWriting = false;
                return EWriteItemErrorCode.InvalidNull;
            }
            if (tempItems.Count is 0)
            {
                IsWriting = false;
                return EWriteItemErrorCode.InvalidEmpty;
            }

            var tempProperties = Properties;

            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                Task.Run(async () =>
                {
                    try
                    {
                        if (tempProperties.RootDirectory is null)
                        {
                            result = EWriteItemErrorCode.TaskRunWritingException;
                            IsWriting = false;
                            return;
                        }
                        if (tempProperties.RootDirectory.Exists is false)
                            tempProperties.RootDirectory.Create();
                        FileInfo fileInfo = new FileInfo(Path.Combine(tempProperties.RootDirectory.FullName, $"{tempProperties.FileName}.{tempProperties.Extension}"));
                        using (var fileStream = new FileStream(fileInfo.FullName, fileInfo.Exists ? FileMode.Append : FileMode.Create))
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            foreach (var item in tempItems)
                                await streamWriter.WriteLineAsync(item);
                        }
                    }
                    catch (Exception)
                    {
                        result = EWriteItemErrorCode.TaskRunWritingException;
                        IsWriting = false;

#if DEBUG
                        throw;
#endif
                    }
                }, cancellationToken);
            }
            catch (Exception)
            {
                result = EWriteItemErrorCode.TaskRunCancellationTokenException;
#if DEBUG
                throw;
#endif
            }
            finally { IsWriting = false; }
            return result;
        }

        /// <summary>
        /// Clears the buffered log entries.
        /// </summary>
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
#if DEBUG
                throw;
#endif
            }
            return result;
        }
    }
}
