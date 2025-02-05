using SimpleFileIO.Log.Csv;
using SimpleFileIO.Log.Text;
using SimpleFileIO.State.Ini;
using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Linq;

namespace SimpleFileIO
{
    /// <summary>
    /// Provides a centralized management interface for different types of logging and state management.
    /// This static class acts as a facade to manage text logs, CSV logs, and INI state configurations.
    /// </summary>
    public static class Manager
    {
        #region TextLog

        /// <summary>
        /// Creates a new text log instance with the specified name and properties.
        /// If a log with the same name already exists, it returns the existing instance.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="properties">Path properties of the log file.</param>
        /// <returns>The created or existing <see cref="ITextLog"/> instance, or <c>null</c> if name is invalid.</returns>
        public static ITextLog? CreateTextLog(string name, PathProperty properties)
        {
            return TextLogManager.Create(name, properties);
        }

        /// <summary>
        /// Adds an existing <see cref="ITextLog"/> instance to the manager.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="instance">Instance of <see cref="ITextLog"/>.</param>
        /// <returns><c>true</c> if added successfully, <c>false</c> if a log with the same name already exists.</returns>
        public static bool AddTextLog(string name, ITextLog instance)
        {
            return TextLogManager.Add(name, instance);
        }

        /// <summary>
        /// Retrieves an existing <see cref="ITextLog"/> instance by name.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <returns>The requested <see cref="ITextLog"/> instance, or <c>null</c> if not found.</returns>
        public static ITextLog? GetTextLog(string name)
        {
            return TextLogManager.Get(name);
        }

        /// <summary>
        /// Retrieves a list of all registered text log names.
        /// </summary>
        /// <returns>A list of log names currently managed.</returns>
        public static List<string> GetTextLogListName()
        {
            return TextLogManager.GetItemListName();
        }

        #endregion

        #region CSVLog

        /// <summary>
        /// Creates a new CSV log instance with the specified name and properties.
        /// If a log with the same name already exists, it returns the existing instance.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="properties">Path properties of the log file.</param>
        /// <returns>The created or existing <see cref="ICSVLog"/> instance, or <c>null</c> if name is invalid.</returns>

        public static ICSVLog? CreateCsvLog(string name, PathProperty properties)
        {
            return CSVLogManager.Create(name, properties);
        }

        /// <summary>
        /// Adds an existing <see cref="ICSVLog"/> instance to the manager.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="instance">Instance of <see cref="ICSVLog"/>.</param>
        /// <returns><c>true</c> if added successfully, <c>false</c> if a log with the same name already exists.</returns>
        public static bool AddCsvLog(string name, ICSVLog instance)
        {
            return CSVLogManager.Add(name, instance);
        }

        /// <summary>
        /// Retrieves an existing <see cref="ICSVLog"/> instance by name.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <returns>The requested <see cref="ICSVLog"/> instance, or <c>null</c> if not found.</returns>
        public static ICSVLog? GetCsvLog(string name)
        {
            return CSVLogManager.Get(name);
        }

        /// <summary>
        /// Retrieves a list of all registered CSV log names.
        /// </summary>
        /// <returns>A list of log names currently managed.</returns>
        public static List<string> GetCsvLogListName()
        {
            return CSVLogManager.GetItemListName();
        }

        #endregion

        #region INIState

        /// <summary>
        /// Creates a new INI state instance with the specified name and properties.
        /// If a state with the same name already exists, it returns the existing instance.
        /// </summary>
        /// <param name="name">Unique name of the state configuration.</param>
        /// <param name="properties">Path properties of the INI file.</param>
        /// <returns>The created or existing <see cref="IINIState"/> instance, or <c>null</c> if name is invalid.</returns>
        public static IINIState? CreateIniState(string name, PathProperty properties)
        {
            return INIStateManager.Create(name, properties);
        }

        /// <summary>
        /// Adds an existing <see cref="IINIState"/> instance to the manager.
        /// </summary>
        /// <param name="name">Unique name of the state configuration.</param>
        /// <param name="instance">Instance of <see cref="IINIState"/>.</param>
        /// <returns><c>true</c> if added successfully, <c>false</c> if a state with the same name already exists.</returns>
        public static bool AddIniState(string name, IINIState instance)
        {
            return INIStateManager.Add(name, instance);
        }

        /// <summary>
        /// Retrieves an existing <see cref="IINIState"/> instance by name.
        /// </summary>
        /// <param name="name">Unique name of the state configuration.</param>
        /// <returns>The requested <see cref="IINIState"/> instance, or <c>null</c> if not found.</returns>
        public static IINIState? GetIniState(string name)
        {
            return INIStateManager.Get(name);
        }

        /// <summary>
        /// Retrieves a list of all registered INI state names.
        /// </summary>
        /// <returns>A list of INI state names currently managed.</returns>
        public static List<string> GetIniStateListName()
        {
            return INIStateManager.GetItemListName();
        }

        #endregion

        static Manager()
        {
        }


    }
}
