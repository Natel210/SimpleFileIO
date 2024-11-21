using SimpleFileIO.Utility;

namespace SimpleFileIO.State.Ini
{
    internal partial class INIState_BaseForm : IINIState
    {
        /// <summary>
        /// gets or sets the file path of the INI file.<br/>
        /// </summary>
        public PathProperty Properties
        {
            get => _properties;
            set
            {
                if (value.RootDirectory != _properties.RootDirectory ||
                    value.FileName != _properties.FileName ||
                    value.Extension != _properties.Extension)
                {
                    _properties = value;
                    Load();
                }
            }
        }

        public string GetValue(string section, string key, string defaultValue)
        {
            if (_sections.ContainsKey(section) is false)
                return defaultValue;
            if (_sections[section].ContainsKey(key) is false)
                return defaultValue;
            string tempValue = _sections[section][key];
            if (string.IsNullOrEmpty(tempValue) is true)
                return defaultValue;
            return tempValue;
        }

        public bool SetValue(string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
                throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            if (!_sections.ContainsKey(section))
                _sections[section] = new Dictionary<string, string>();

            _sections[section][key] = value;
            return true;
        }

        public string GetValue(ref IniItem<string> item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (!item.IsValidSectionKey)
                return item.DefaultValue;
            item.Value = GetValue(item.Section, item.Key, item.DefaultValue);
            return item.Value;
        }

        public bool SetValue(IniItem<string> item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (!item.IsValidSectionKey)
                return false;
            return SetValue(item.Section, item.Key, item.Value);
        }

        /// <summary>
        /// saves changes to the INI file.
        /// </summary>
        public bool Save()
        {
            if (_isWriting)
                return false;
            if (!CheckPathProperty(Properties))
                return false;
            _isWriting = true;
            // async run
            Task.Run(async () =>
            {
                try
                {
                    // saving tempProperty, use task
                    PathProperty tempPathProperty = Properties;

                    // Directory check and create if necessary
                    if (!tempPathProperty.RootDirectory.Exists)
                        tempPathProperty.RootDirectory.Create();

                    FileInfo fileInfo = new FileInfo(Path.Combine(tempPathProperty.RootDirectory.FullName, $"{tempPathProperty.FileName}.{tempPathProperty.Extension}"));
                    using (var fileStream = new FileStream(fileInfo.FullName, fileInfo.Exists ? FileMode.Append : FileMode.Create))
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
                catch (Exception)
                {
#if DEBUG
                    throw;
#endif
                }
                finally
                {
                    _isWriting = false; // 작업이 끝나면 플래그를 해제
                }
            });

            return true;
        }

        /// <summary>
        /// loads the INI file.
        /// </summary>
        /// <returns>true if the file was successfully loaded; otherwise, false.</returns>
        public bool Load()
        {
            if (_isLoading is true)
                return false;
            if (CheckPathProperty(Properties) is false)
                return false;
            if (CheckFileExist() is false)
                return false;
            // Directory check and create if necessary
            if (Directory.Exists("./TempIniData/") is false)
                Directory.CreateDirectory("./TempIniData/");

            PathProperty tempPathProperty = Properties;
            var getCurTick = DateTime.UtcNow.Ticks;
            FileInfo originFileInfo = new FileInfo(Path.Combine(tempPathProperty.RootDirectory.FullName, $"{tempPathProperty.FileName}.{tempPathProperty.Extension}"));
            FileInfo tempCopyFileInfo = new FileInfo(Path.Combine("./TempIniData/", $"{tempPathProperty.FileName}_{getCurTick}.{tempPathProperty.Extension}"));
            originFileInfo.CopyTo(tempCopyFileInfo.FullName, true);

            _isLoading = true;

            // async run
            Task.Run(async () =>
            {
                try
                {
                    // Copy the original file to a temporary file for safe reading


                    using (var fileStream = new FileStream(tempCopyFileInfo.FullName, FileMode.Open, FileAccess.Read))
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        string currentSection = "";
                        while (streamReader.EndOfStream is false)
                        {
                            string? line = await streamReader.ReadLineAsync(); // Asynchronously read a line
                            if (string.IsNullOrWhiteSpace(line) is true)
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
                catch (Exception)
                {
#if DEBUG
                    throw;
#endif
                }
                finally
                {
                    _isLoading = false; // 작업이 끝나면 플래그를 해제
                }

            });
            if (tempCopyFileInfo.Exists is true)
                tempCopyFileInfo.Delete();
            
            return true;
        }

        /// <summary>
        /// checks if the INI file exists.
        /// </summary>
        /// <returns>true if the file exists; otherwise, false.</returns>
        public bool CheckFileExist()
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(Properties.RootDirectory.FullName, $"{Properties.FileName}.{Properties.Extension}"));
            return File.Exists(fileInfo.FullName);
        }

        public T GetValue_UseParser<T>(string section, string key, T defaultValue) where T : notnull
        {
            StringTypeParser parser;
            if (_stringTypeParsers.TryGetValue(typeof(T), out parser) is false)
                return defaultValue;
            if (parser.StringToObject is null)
                return defaultValue;
            if (parser.ObjectToString is null)
                return defaultValue;
            string? defaultValueString = parser.ObjectToString(defaultValue);
            if (string.IsNullOrWhiteSpace(defaultValueString) is true)
                return defaultValue;
            string getValueString = GetValue(section, key, defaultValueString);
            if (string.IsNullOrWhiteSpace(getValueString) is true)
                return defaultValue;
            if (parser.StringToObject(getValueString) is T resultValue)
                return resultValue;
            else
                return defaultValue;
        }

        public bool SetValue_UseParser<T>(string section, string key, T value) where T : notnull
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

        public T GetValue_UseParser<T>(ref IniItem<T> item) where T : notnull
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
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

        public bool SetValue_UseParser<T>(IniItem<T> item) where T : notnull
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

        public bool RemoveParser(Type type)
        {
            return _stringTypeParsers.Remove(type);
        }

    }

    internal partial class INIState_BaseForm
    {
        private PathProperty _properties = new();
        private readonly Dictionary<string, Dictionary<string, string>> _sections = new();
        private readonly Dictionary<Type, StringTypeParser> _stringTypeParsers = new ();
        private bool _isWriting = false;
        private bool _isLoading = false;

        internal INIState_BaseForm() : base()
        {
            AddParsers();
        }

        internal INIState_BaseForm(string path) : base()
        {
            _properties.RootDirectory = new(Path.GetDirectoryName(path) ?? string.Empty);
            _properties.FileName = Path.GetFileNameWithoutExtension(path);
            _properties.Extension = Path.GetExtension(path);
            AddParsers();
        }

        private bool CheckPathProperty(PathProperty pathProperty)
        {
            if (pathProperty.RootDirectory is null)
#if DEBUG
                throw new ArgumentNullException($"Check PathProperty [{nameof(pathProperty.RootDirectory)}] null.");
#else
                    return false;
#endif
            if (string.IsNullOrWhiteSpace(pathProperty.RootDirectory.FullName))
#if DEBUG
                throw new ArgumentNullException($"Check PathProperty [{nameof(pathProperty.RootDirectory)}].FullName null or empty.");
#else
                    return false;
#endif

            if (string.IsNullOrWhiteSpace(pathProperty.FileName))
#if DEBUG
                throw new ArgumentNullException($"Check PathProperty [{nameof(pathProperty.FileName)}] null or empty.");
#else
                    return false;
#endif

            if (string.IsNullOrWhiteSpace(pathProperty.Extension))
#if DEBUG
                throw new ArgumentNullException($"Check PathProperty [{nameof(pathProperty.Extension)}] null or empty.");
#else
                    return false;
#endif
            return true;
        }

    }

    internal partial class INIState_BaseForm
    {
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
                    if (bool.TryParse(str, out bool result))
                        return result;
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
                        //Path.Combine(,)



                        return string.Join(", ", stringlist);
                    }
                    else
                        return null;

                },
                StringToObject = (str) =>
                {
                    if (bool.TryParse(str, out bool result))
                        return result;
                    return str.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).ToArray();
                }
            });

        }
    }
}
