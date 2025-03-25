using SimpleFileIO.Resources.ErrorMessages;
using SimpleFileIO.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleFileIO.State.Ini
{
    /// <summary>
    /// Provides a basic implementation of the <see cref="IINIState"/> interface for INI-based logging.
    /// This class offers functionality to manage log entries, write them to a file, and clear them.
    /// It provides default values internally and allows customization through inheritance.
    /// </summary>
    internal partial class INIState_BaseForm : IINIState
    {
        /// Provides a basic implementation of the <see cref="IINIState"/> interface for INI-based logging.
        /// This class offers functionality to manage log entries, write them to a file, and clear them.
        /// It provides default values internally and allows customization through inheritance.
        private PathProperty _pathProperties = new();

        /// <summary>
        /// Mutex object to ensure thread safety for log property operations.
        /// </summary>
        private readonly object _pathPropertiesMutex = new object();

        /// <summary>
        /// Dictionary storing section and key-value pairs of the INI file.
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, string>> _sections = new();

        /// <summary>
        /// Mutex object to ensure thread safety for content entry operations.
        /// </summary>
        private readonly object _sectionsMutex = new object();

        /// <summary>
        /// Dictionary storing type parsers for converting values.
        /// </summary>
        private readonly Dictionary<Type, StringTypeParser> _stringTypeParsers = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="INIState_BaseForm"/> class.
        /// </summary>
        internal INIState_BaseForm() : base()
        {
            AddParsers();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="INIState_BaseForm"/> class with a specified file path.
        /// </summary>
        /// <param name="path">The file path of the INI file.</param>
        internal INIState_BaseForm(string path) : base()
        {
            lock (_pathPropertiesMutex)
            {
                _pathProperties.RootDirectory = new(Path.GetDirectoryName(path) ?? string.Empty);
                _pathProperties.FileName = Path.GetFileNameWithoutExtension(path);
                _pathProperties.Extension = Path.GetExtension(path).TrimStart('.');
            }
            AddParsers();
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
        /// Gets or sets the file path properties of the INI file.
        /// This includes the root directory, file name, and extension.
        /// </summary>
        public PathProperty PathProperty
        {
            get
            {
                PathProperty result = new PathProperty();
                try
                {
                    lock (_pathPropertiesMutex)
                    {
                        result = new PathProperty();
                        if (_pathProperties.RootDirectory is not null)
                            result.RootDirectory = new DirectoryInfo(_pathProperties.RootDirectory.FullName);
                        result.FileName = _pathProperties.FileName;
                        result.Extension = _pathProperties.Extension;
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
                if (value.RootDirectory != _pathProperties.RootDirectory ||
                    value.FileName != _pathProperties.FileName ||
                    value.Extension != _pathProperties.Extension)
                {
                    try
                    {
                        lock (_pathPropertiesMutex)
                        {
                            _pathProperties.RootDirectory = new DirectoryInfo(value.RootDirectory.FullName);
                            _pathProperties.FileName = value.FileName;
                            _pathProperties.Extension = value.Extension;
                        }
                        Load();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ErrorMessages.path_property_work);
                        if (ThrowExceptionMode is true)
                            throw new InvalidOperationException(ErrorMessages.path_property_work, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the logging process is currently in progress.
        /// </summary>
        /// <value>
        /// <c>true</c> if logging is in progress; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaving { get; private set; } = false;

        /// <summary>
        /// Indicates whether the INI file is currently being loaded.
        /// </summary>
        public bool IsLoading { get; private set; } = false;

        /// <summary>
        /// Retrieves a string value from the specified section and key in the INI file.
        /// If the key does not exist, the provided default value is returned.
        /// </summary>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key within the section.</param>
        /// <param name="defaultValue">The default value to return if the key does not exist.</param>
        /// <returns>The retrieved string value, or the default value if the key is missing.</returns>
        public string GetValue(string section, string key, string defaultValue)
        {
            if (_sections.ContainsKey(section) is false)
                return defaultValue;
            if (_sections[section].ContainsKey(key) is false)
                return defaultValue;
            lock (_sectionsMutex)
            {
                string tempValue = _sections[section][key];
                if (string.IsNullOrEmpty(tempValue) is true)
                    return defaultValue;
                return tempValue;
            }
        }

        /// <summary>
        /// Retrieves a string value from an <see cref="IniItem{T}"/> reference.
        /// </summary>
        /// <param name="item">The INI item containing the section and key reference.</param>
        /// <returns>The retrieved value from the INI file.</returns>
        public string GetValue(ref IniItem<string> item)
        {
            if (item is null)
            {
                Debug.WriteLine(ErrorMessages.get_value_invalid_item);
                if (ThrowExceptionMode is true)
                    throw new ArgumentNullException(ErrorMessages.get_value_invalid_item);
                return "";
            }
            if (!item.IsValidSectionKey)
                return item.DefaultValue;
            item.Value = GetValue(item.Section, item.Key, item.DefaultValue);
            return item.Value;
        }

        /// <summary>
        /// Sets a string value for the specified section and key in the INI file.
        /// </summary>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key to update or create.</param>
        /// <param name="value">The value to store.</param>
        /// <returns><c>true</c> if the value was successfully set; otherwise, <c>false</c>.</returns>
        public bool SetValue(string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
            {
                string errorMessage = $"{ErrorMessages.set_value_invaild_section},{nameof(section)}";
                Debug.WriteLine(errorMessage);
                if (ThrowExceptionMode is true)
                    throw new ArgumentNullException(errorMessage);
                return false;
            }

            if (string.IsNullOrEmpty(key))
            {
                string errorMessage = $"{ErrorMessages.set_value_invaild_key},{nameof(key)}";
                Debug.WriteLine(errorMessage);
                if (ThrowExceptionMode is true)
                    throw new ArgumentNullException(errorMessage);
                return false;
            }

            //if (string.IsNullOrEmpty(value))
            //{
            //    string errorMessage = $"{ErrorMessages.set_value_invaild_value},{nameof(value)}";
            //    Debug.WriteLine(errorMessage);
            //    if (ThrowExceptionMode is true)
            //        throw new ArgumentNullException(errorMessage);
            //    return false;
            //}

            lock (_sectionsMutex)
            {
                if (!_sections.ContainsKey(section))
                    _sections[section] = new Dictionary<string, string>();
                _sections[section][key] = value;
                return true;
            }
        }

        /// <summary>
        /// Sets a value for an <see cref="IniItem{T}"/> entry.
        /// </summary>
        /// <param name="item">The INI item to update.</param>
        /// <returns><c>true</c> if the value was successfully set; otherwise, <c>false</c>.</returns>
        public bool SetValue(IniItem<string> item)
        {
            if (item is null)
            {
                Debug.WriteLine(ErrorMessages.get_value_invalid_item);
                if (ThrowExceptionMode is true)
                    throw new ArgumentNullException(ErrorMessages.set_value_invalid_item);
                return false;
            }
            if (!item.IsValidSectionKey)
                return false;
            return SetValue(item.Section, item.Key, item.Value);
        }

        /// <summary>
        /// Saves any modifications to the INI file.
        /// </summary>
        /// <returns><c>true</c> if the file was successfully saved; otherwise, <c>false</c>.</returns>
        public bool Save()
        {
            if (IsSaving)
            {
                Debug.WriteLine(ErrorMessages.write_already_writing);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.write_already_writing);
                return false;
            }
            if (!CheckPathProperty(PathProperty))
                return false;
            IsSaving = true;
            Dictionary<string, Dictionary<string, string>> tempSections;
            try
            {
                lock (_sectionsMutex)
                {
                    if (_sections.Count != 0)
                    {
                        tempSections = new Dictionary<string, Dictionary<string, string>>();
                        foreach (var item in _sections)
                            tempSections.Add(item.Key, new Dictionary<string,string>(item.Value));
                        if (tempSections == null || tempSections.Count == 0)
                        {
                            Debug.WriteLine(ErrorMessages.save_deep_copying);
                            throw new InvalidOperationException(ErrorMessages.save_deep_copying);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                IsSaving = false;
                Debug.WriteLine(ErrorMessages.save_deep_copying);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.save_deep_copying, ex);
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
                            Debug.WriteLine(ErrorMessages.save_task_root_directory_not_found);
                            throw new DirectoryNotFoundException(ErrorMessages.save_task_root_directory_not_found);
                        }

                        // Directory check and create if necessary
                        if (!tempProperties.RootDirectory.Exists)
                            tempProperties.RootDirectory.Create();
                        FileInfo fileInfo = new FileInfo(Path.Combine(tempProperties.RootDirectory.FullName, $"{tempProperties.FileName}.{tempProperties.Extension}"));
                        using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            foreach (var section in _sections)
                            {
                                await streamWriter.WriteLineAsync($"[{section.Key}]");
                                foreach (var keyValue in section.Value)
                                    await streamWriter.WriteLineAsync($"{keyValue.Key} = {keyValue.Value}");
                                await streamWriter.WriteLineAsync();
                            }
                        }
                    }
                    catch (Exception ex )
                    {
                        isError = true;
                        IsSaving = false;
                        Debug.WriteLine(ErrorMessages.save_task_work);
                        if (ThrowExceptionMode is true)
                            throw new InvalidOperationException(ErrorMessages.save_task_work, ex);
                        return;
                    }
                }).Wait();
            }
            catch (Exception ex)
            {
                isError = true;
                IsSaving = false;
                Debug.WriteLine(ErrorMessages.save_task_cancellation_token);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.save_task_cancellation_token, ex);
                return false;
            }


            IsSaving = false;
            return !isError;
        }

        /// <summary>
        /// Loads the contents of the INI file into memory.
        /// </summary>
        /// <returns><c>true</c> if the file was successfully loaded; otherwise, <c>false</c>.</returns>
        public bool Load()
        {
            if (IsLoading)
                return false;
            if (!CheckPathProperty(PathProperty))
                return false;
            if (!CheckFileExist())
                return false;

            IsLoading = true;

            PathProperty tempPathProperty = PathProperty;
            var getCurTick = DateTime.UtcNow.Ticks;
            FileInfo originFileInfo = new FileInfo(Path.Combine(tempPathProperty.RootDirectory.FullName, $"{tempPathProperty.FileName}.{tempPathProperty.Extension}"));
            FileInfo tempCopyFileInfo = new FileInfo(Path.Combine(tempPathProperty.RootDirectory.FullName, $"{tempPathProperty.FileName}_temp{getCurTick}.{tempPathProperty.Extension}"));

            // copy from origin
            originFileInfo.CopyTo(tempCopyFileInfo.FullName, true);
            bool isError = false;
            try
            {
                // run async
                Task.Run(async () =>
                {
                    try
                    {
                        using (var fileStream = new FileStream(tempCopyFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            string currentSection = "";
                            while (!streamReader.EndOfStream)
                            {
                                string? line = await streamReader.ReadLineAsync();
                                if (string.IsNullOrWhiteSpace(line))
                                    continue;

                                string trimmedLine = line.Trim();
                                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                                {
                                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                                    if (!_sections.ContainsKey(currentSection))
                                        _sections[currentSection] = new Dictionary<string, string>();
                                }
                                else if (trimmedLine.Contains("=") && !trimmedLine.StartsWith(";") && currentSection != "")
                                {
                                    string[] parts = trimmedLine.Split('=', 2);
                                    _sections[currentSection][parts[0].Trim()] = parts[1].Trim();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        IsLoading = false;
                        Debug.WriteLine(ErrorMessages.load_task_work);
                        if (ThrowExceptionMode is true)
                            throw new InvalidOperationException(ErrorMessages.load_task_work, ex);
                        return;
                    }
                }).Wait();
                //Delete Temp File
                if (tempCopyFileInfo.Exists)
                {
                    try
                    {
                        tempCopyFileInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ErrorMessages.load_task_delete_temp_file);
                        if (ThrowExceptionMode is true)
                            throw new InvalidOperationException(ErrorMessages.load_task_delete_temp_file, ex);
                    }
                }

            }
            catch (Exception ex)
            {
                isError = true;
                IsLoading = false;
                Debug.WriteLine(ErrorMessages.load_task_cancellation_token);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.load_task_cancellation_token, ex);
                return false;
            }
            IsLoading = false;
            return !isError;
        }


        /// <summary>
        /// Checks if the INI file exists at the specified path.
        /// </summary>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        public bool CheckFileExist()
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(PathProperty.RootDirectory.FullName, $"{PathProperty.FileName}.{PathProperty.Extension}"));
            return File.Exists(fileInfo.FullName);
        }

        /// <summary>
        /// Retrieves a value from the INI file and converts it to the specified type using a registered parser.
        /// If the key does not exist or conversion fails, the default value is returned.
        /// </summary>
        /// <typeparam name="T">The target type for conversion (must have a public parameterless constructor).</typeparam>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key within the section.</param>
        /// <param name="defaultValue">The default value to return if retrieval or conversion fails.</param>
        /// <returns>The retrieved and parsed value, or the default value if not found.</returns>
        public T GetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(string section, string key, T defaultValue) where T : notnull
        {
            StringTypeParser parser;
            if (_stringTypeParsers.TryGetValue(typeof(T), out parser) is false)
                return defaultValue;
            if (parser.StringToObject is null)
                return defaultValue;
            if (parser.ObjectToString is null)
                return defaultValue;
            string? defaultValueString = parser.ObjectToString(defaultValue);
            //if (string.IsNullOrWhiteSpace(defaultValueString) is true)
            //    return defaultValue;
            string getValueString = GetValue(section, key, defaultValueString ?? "");
            if (string.IsNullOrWhiteSpace(getValueString) is true)
                return defaultValue;
            if (parser.StringToObject(getValueString) is T resultValue)
                return resultValue;
            else
                return defaultValue;
        }

        /// <summary>
        /// Retrieves a value from an <see cref="IniItem{T}"/> reference and converts it using a registered parser.
        /// </summary>
        /// <typeparam name="T">The target type for conversion (must have a public constructor).</typeparam>
        /// <param name="item">The INI item containing the section and key reference.</param>
        /// <returns>The retrieved and parsed value.</returns>
        public T GetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(ref IniItem<T> item) where T : notnull
        {
            if (item is null)
            {
                string errorMessage = $"{ErrorMessages.get_value_invalid_item}, {typeof(T).Name}";
                Debug.WriteLine(errorMessage);
                if (ThrowExceptionMode is true)
                    throw new ArgumentNullException(errorMessage);
                return default!;
            }

            if (!item.IsValidSectionKey)
                return item.DefaultValue;

            T tempValue = item.Value;
            item.Value = item.DefaultValue;

            StringTypeParser parser;
            if (_stringTypeParsers.TryGetValue(typeof(T), out parser) is false)
                return item.DefaultValue;
            if (parser.StringToObject is null)
                return item.DefaultValue;
            if (parser.ObjectToString is null)
                return item.DefaultValue;
            string? defaultValueString = parser.ObjectToString(item.DefaultValue);
            if (string.IsNullOrWhiteSpace(defaultValueString) is true)
                return item.DefaultValue;

            IniItem<string> tempItem = new();
            tempItem.Section = item.Section;
            tempItem.Key = item.Key;
            tempItem.Value = parser.ObjectToString(item.Value) ?? "";
            tempItem.DefaultValue = defaultValueString;

            if (string.IsNullOrWhiteSpace(GetValue(ref tempItem)) is true)
                return item.DefaultValue;

            if (parser.StringToObject(tempItem.Value) is T resultValue)
            {
                item.Value = resultValue;
                return resultValue;
            }
            else
                return item.DefaultValue;
        }

        /// <summary>
        /// Sets a typed value in the INI file using a registered parser.
        /// The value is converted to a string before storage.
        /// </summary>
        /// <typeparam name="T">The type of the value to store (must have a public parameterless constructor).</typeparam>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key to store the value under.</param>
        /// <param name="value">The value to store.</param>
        /// <returns><c>true</c> if the value was successfully stored; otherwise, <c>false</c>.</returns>
        public bool SetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(string section, string key, T value) where T : notnull
        {
            StringTypeParser parser;
            if (_stringTypeParsers.TryGetValue(typeof(T), out parser) is false)
                return false;
            if (parser.ObjectToString is null)
                return false;
            string? valueString = parser.ObjectToString(value);
            if (string.IsNullOrWhiteSpace(valueString) is true)
                return false;
            return SetValue(section, key, valueString);
        }

        /// <summary>
        /// Sets a typed value for an <see cref="IniItem{T}"/> entry using a registered parser.
        /// </summary>
        /// <typeparam name="T">The type of the value to store (must have a public constructor).</typeparam>
        /// <param name="item">The INI item to update.</param>
        /// <returns><c>true</c> if the value was successfully set; otherwise, <c>false</c>.</returns>
        public bool SetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IniItem<T> item) where T : notnull
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (!item.IsValidSectionKey)
                return false;

            StringTypeParser parser;
            if (_stringTypeParsers.TryGetValue(typeof(T), out parser) is false)
                return false;
            if (parser.ObjectToString is null)
                return false;
            string? valueString = parser.ObjectToString(item.Value);
            if (string.IsNullOrWhiteSpace(valueString) is true)
                return false;

            IniItem<string> tempItem = new();
            tempItem.Section = item.Section;
            tempItem.Key = item.Key;
            tempItem.Value = valueString;
            tempItem.DefaultValue = parser.ObjectToString(item.DefaultValue) ?? "";

            return SetValue(tempItem);
        }

        /// <summary>
        /// Registers a custom parser for a specific type, allowing conversion between string and object representations.
        /// </summary>
        /// <param name="type">The type to register the parser for.</param>
        /// <param name="parser">The <see cref="StringTypeParser"/> containing conversion functions.</param>
        /// <param name="overwrite">If <c>true</c>, overwrites an existing parser for the type.</param>
        /// <returns><c>true</c> if the parser was successfully added; otherwise, <c>false</c>.</returns>
        public bool AddParser(Type type, StringTypeParser parser, bool overwrite = false)
        {
            if (overwrite)
                _stringTypeParsers[type] = parser;
            else
            {
                if (_stringTypeParsers.ContainsKey(type) is true)
                    return false;
                _stringTypeParsers.Add(type, parser);
            }
            return true;
        }

        /// <summary>
        /// Removes a registered parser for a specific type.
        /// </summary>
        /// <param name="type">The type whose parser should be removed.</param>
        /// <returns><c>true</c> if the parser was successfully removed; otherwise, <c>false</c>.</returns>
        public bool RemoveParser(Type type)
        {
            return _stringTypeParsers.Remove(type);
        }

    }

    internal partial class INIState_BaseForm
    {


        /// <summary>
        /// Checks if the given path property is valid.
        /// </summary>
        /// <param name="pathProperty">The path property to check.</param>
        /// <returns>Returns true if the path property is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any part of the path property is null or empty.</exception>
        private bool CheckPathProperty(PathProperty pathProperty)
        {
            if (pathProperty.RootDirectory is null)
            {
                Debug.WriteLine(ErrorMessages.check_path_property_invaild_root_direcroty);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.check_path_property_invaild_root_direcroty);
                return false;
            }
            if (string.IsNullOrWhiteSpace(pathProperty.RootDirectory.FullName))
            {
                Debug.WriteLine(ErrorMessages.check_path_property_invaild_root_direcroty);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.check_path_property_invaild_root_direcroty);
                return false;
            }
            if (string.IsNullOrWhiteSpace(pathProperty.FileName))
            {
                Debug.WriteLine(ErrorMessages.check_path_property_invaild_file_name);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.check_path_property_invaild_file_name);
                return false;
            }
            if (string.IsNullOrWhiteSpace(pathProperty.Extension))
            {
                Debug.WriteLine(ErrorMessages.check_path_property_invaild_extension);
                if (ThrowExceptionMode is true)
                    throw new InvalidOperationException(ErrorMessages.check_path_property_invaild_extension);
                return false;
            }
            return true;
        }

    }

    internal partial class INIState_BaseForm
    {
        /// <summary>
        /// Base Parsers Forms.
        /// </summary>
        private void AddParsers()
        {
            //Integer Types
            AddParser(typeof(byte), new()
            {
                TargetType = typeof(byte),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (byte.TryParse(str, out byte result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(sbyte), new()
            {
                TargetType = typeof(sbyte),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (sbyte.TryParse(str, out sbyte result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(short), new()
            {
                TargetType = typeof(short),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (short.TryParse(str, out short result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(ushort), new()
            {
                TargetType = typeof(ushort),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (ushort.TryParse(str, out ushort result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(int), new()
            {
                TargetType = typeof(int),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (int.TryParse(str, out int result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(uint), new()
            {
                TargetType = typeof(uint),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (uint.TryParse(str, out uint result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(long), new()
            {
                TargetType = typeof(long),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (long.TryParse(str, out long result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(ulong), new()
            {
                TargetType = typeof(ulong),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (ulong.TryParse(str, out ulong result))
                        return result;
                    return null;
                }
            });

            //Floating-Point Types
            AddParser(typeof(float), new()
            {
                TargetType = typeof(float),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (float.TryParse(str, out float result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(double), new()
            {
                TargetType = typeof(double),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (double.TryParse(str, out double result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(decimal), new()
            {
                TargetType = typeof(decimal),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (decimal.TryParse(str, out decimal result))
                        return result;
                    return null;
                }
            });

            //Character Type
            AddParser(typeof(char), new()
            {
                TargetType = typeof(char),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (char.TryParse(str, out char result))
                        return result;
                    return null;
                }
            });
            AddParser(typeof(string), new()
            {
                TargetType = typeof(string),
                ObjectToString = (obj) => (string)obj,
                StringToObject = (str) => str
            });

            //Boolean Type
            AddParser(typeof(bool), new()
            {
                TargetType = typeof(bool),
                ObjectToString = (obj) => obj.ToString(),
                StringToObject = (str) =>
                {
                    if (bool.TryParse(str, out bool result))
                        return result;
                    return null;
                }
            });

            //Special Types
            AddParser(typeof(string[]), new()
            {
                TargetType = typeof(string[]),
                ObjectToString = (obj) =>
                {
                    if (obj is string[] stringlist)
                        return string.Join(", ", stringlist);
                    else
                        return null;

                },
                StringToObject = (str) =>
                {
                    return str.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).ToArray();
                }
            });
            AddParser(typeof(PathProperty), new()
            {
                TargetType = typeof(PathProperty),
                ObjectToString = (obj) =>
                {
                    if (obj is PathProperty stringlist)
                    {
                        string getRootDirectoryFullPath = "";
                        if (stringlist.RootDirectory is not null)
                            getRootDirectoryFullPath = stringlist.RootDirectory.FullName;
                        else
                            getRootDirectoryFullPath = "./";
                        return $"{getRootDirectoryFullPath}/{stringlist.FileName}.{stringlist.Extension}";
                    }
                    else
                        return null;

                },
                StringToObject = (str) =>
                {
                    var result = new PathProperty();
                    result.RootDirectory = new(Path.GetDirectoryName(str) ?? string.Empty);
                    result.FileName = Path.GetFileNameWithoutExtension(str);
                    result.Extension = Path.GetExtension(str).TrimStart('.');
                    return result;
                }
            });

        }
    }
}
