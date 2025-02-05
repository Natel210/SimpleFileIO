using SimpleFileIO.Utility;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleFileIO.State.Ini
{
    /// <summary>
    /// Defines an interface for managing INI file operations.
    /// This interface allows reading, writing, and maintaining structured data within an INI file format.
    /// It also provides support for parsing custom object types.
    /// </summary>
    public partial interface IINIState
    {
        /// <summary>
        /// Gets or sets the file path properties of the INI file.
        /// This includes the root directory, file name, and extension.
        /// </summary>
        PathProperty PathProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether exceptions should be thrown when an error occurs.
        /// </summary>
        /// <value>
        /// <c>true</c> if exceptions should be thrown; otherwise, <c>false</c>. 
        /// When set to <c>false</c>, errors will be handled internally without throwing exceptions.
        /// </value>
        public bool ThrowExceptionMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the logging process is currently in progress.
        /// </summary>
        /// <value>
        /// <c>true</c> if logging is in progress; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaving { get; }

        /// <summary>
        /// Indicates whether the INI file is currently being loaded.
        /// </summary>
        public bool IsLoading { get; }

        /// <summary>
        /// Retrieves a string value from the specified section and key in the INI file.
        /// If the key does not exist, the provided default value is returned.
        /// </summary>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key within the section.</param>
        /// <param name="defaultValue">The default value to return if the key does not exist.</param>
        /// <returns>The retrieved string value, or the default value if the key is missing.</returns>
        string GetValue(string section, string key, string defaultValue);

        /// <summary>
        /// Retrieves a string value from an <see cref="IniItem{T}"/> reference.
        /// </summary>
        /// <param name="item">The INI item containing the section and key reference.</param>
        /// <returns>The retrieved value from the INI file.</returns>
        string GetValue(ref IniItem<string> item);

        /// <summary>
        /// Sets a string value for the specified section and key in the INI file.
        /// </summary>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key to update or create.</param>
        /// <param name="value">The value to store.</param>
        /// <returns><c>true</c> if the value was successfully set; otherwise, <c>false</c>.</returns>
        bool SetValue(string section, string key, string value);

        /// <summary>
        /// Sets a value for an <see cref="IniItem{T}"/> entry.
        /// </summary>
        /// <param name="item">The INI item to update.</param>
        /// <returns><c>true</c> if the value was successfully set; otherwise, <c>false</c>.</returns>
        bool SetValue(IniItem<string> item);

        /// <summary>
        /// Saves any modifications to the INI file.
        /// </summary>
        /// <returns><c>true</c> if the file was successfully saved; otherwise, <c>false</c>.</returns>
        bool Save();

        /// <summary>
        /// Loads the contents of the INI file into memory.
        /// </summary>
        /// <returns><c>true</c> if the file was successfully loaded; otherwise, <c>false</c>.</returns>
        bool Load();

        /// <summary>
        /// Checks if the INI file exists at the specified path.
        /// </summary>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        bool CheckFileExist();

    }

    /// <summary>
    /// Defines additional methods for managing typed values in an INI file using custom parsers.
    /// These methods allow automatic conversion of stored string values to specific types.
    /// </summary>
    public partial interface IINIState
    {
        /// <summary>
        /// Retrieves a value from the INI file and converts it to the specified type using a registered parser.
        /// If the key does not exist or conversion fails, the default value is returned.
        /// </summary>
        /// <typeparam name="T">The target type for conversion (must have a public parameterless constructor).</typeparam>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key within the section.</param>
        /// <param name="defaultValue">The default value to return if retrieval or conversion fails.</param>
        /// <returns>The retrieved and parsed value, or the default value if not found.</returns>
        T GetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]  T>(string section, string key, T defaultValue) where T : notnull;

        /// <summary>
        /// Retrieves a value from an <see cref="IniItem{T}"/> reference and converts it using a registered parser.
        /// </summary>
        /// <typeparam name="T">The target type for conversion (must have a public constructor).</typeparam>
        /// <param name="item">The INI item containing the section and key reference.</param>
        /// <returns>The retrieved and parsed value.</returns>
        T GetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(ref IniItem<T> item) where T : notnull;

        /// <summary>
        /// Sets a typed value in the INI file using a registered parser.
        /// The value is converted to a string before storage.
        /// </summary>
        /// <typeparam name="T">The type of the value to store (must have a public parameterless constructor).</typeparam>
        /// <param name="section">The section of the INI file.</param>
        /// <param name="key">The key to store the value under.</param>
        /// <param name="value">The value to store.</param>
        /// <returns><c>true</c> if the value was successfully stored; otherwise, <c>false</c>.</returns>
        bool SetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]  T>(string section, string key, T value) where T : notnull;

        /// <summary>
        /// Sets a typed value for an <see cref="IniItem{T}"/> entry using a registered parser.
        /// </summary>
        /// <typeparam name="T">The type of the value to store (must have a public constructor).</typeparam>
        /// <param name="item">The INI item to update.</param>
        /// <returns><c>true</c> if the value was successfully set; otherwise, <c>false</c>.</returns>
        bool SetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IniItem<T> item) where T : notnull;

        /// <summary>
        /// Registers a custom parser for a specific type, allowing conversion between string and object representations.
        /// </summary>
        /// <param name="type">The type to register the parser for.</param>
        /// <param name="parser">The <see cref="StringTypeParser"/> containing conversion functions.</param>
        /// <param name="overwrite">If <c>true</c>, overwrites an existing parser for the type.</param>
        /// <returns><c>true</c> if the parser was successfully added; otherwise, <c>false</c>.</returns>
        bool AddParser(Type type, StringTypeParser parser, bool overwrite = false);

        /// <summary>
        /// Removes a registered parser for a specific type.
        /// </summary>
        /// <param name="type">The type whose parser should be removed.</param>
        /// <returns><c>true</c> if the parser was successfully removed; otherwise, <c>false</c>.</returns>
        bool RemoveParser(Type type);
        

    }
}
