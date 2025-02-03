using SimpleFileIO.Utility;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleFileIO.State.Ini
{
    public partial interface IINIState
    {
        /// <summary>
        /// gets or sets the file path of the INI file.
        /// </summary>
        PathProperty Properties { get; set; }

        string GetValue(string section, string key, string defaultValue);
        bool SetValue(string section, string key, string value);

        string GetValue(ref IniItem<string> item);
        bool SetValue(IniItem<string> item);

        /// <summary>
        /// saves changes to the INI file.
        /// </summary>
        bool Save();

        /// <summary>
        /// loads the INI file.
        /// </summary>
        /// <returns>true if the file was successfully loaded; otherwise, false.</returns>
        bool Load();

        /// <summary>
        /// checks if the INI file exists.
        /// </summary>
        /// <returns>true if the file exists; otherwise, false.</returns>
        bool CheckFileExist();

    }

    public partial interface IINIState
    {
        T GetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]  T>(string section, string key, T defaultValue) where T : notnull;
        bool SetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]  T>(string section, string key, T value) where T : notnull;

        T GetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(ref IniItem<T> item) where T : notnull;
        bool SetValue_UseParser<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IniItem<T> item) where T : notnull;

        bool AddParser(Type type, StringTypeParser parser, bool overwrite = false);
        bool RemoveParser(Type type);
        

    }
}
