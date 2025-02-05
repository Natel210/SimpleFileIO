using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Threading;

namespace SimpleFileIO.Log.Csv
{
    /// <summary>
    /// Manages multiple instances of CSV-based logs in a thread-safe manner.
    /// This static class provides functionality for creating, retrieving, and managing CSV log instances.
    /// </summary>
    internal static class CSVLogManager
    {
        /// <summary>
        /// Stores instances of <see cref="ICSVLog"/> using a dictionary with unique names as keys.
        /// </summary>
        private static Dictionary<string, ICSVLog> _itemDic = new();

        /// <summary>
        /// Ensures thread safety when accessing or modifying <see cref="_itemDic"/>.
        /// </summary>
        private static Mutex _itemDicMutex = new();

        /// <summary>
        /// Creates a new CSV log instance with the specified name and properties.
        /// If a log with the same name already exists, it returns the existing instance.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="properties">Path properties of the log file.</param>
        /// <returns>The created or existing <see cref="ICSVLog"/> instance, or <c>null</c> if name is invalid.</returns>
        internal static ICSVLog? Create(string name, PathProperty properties)
        {
            if (name is null)
                return null;

            if (Exist(name))
                return Get(name);

            CSVLog_BaseForm addItem = new CSVLog_BaseForm();
            addItem.PathProperty = properties;
            _itemDic.Add(name, addItem);
            return Get(name);
        }

        /// <summary>
        /// Adds an existing <see cref="ICSVLog"/> instance to the manager.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="instance">Instance of <see cref="ICSVLog"/>.</param>
        /// <returns><c>true</c> if added successfully, <c>false</c> if a log with the same name already exists.</returns>
        internal static bool Add(string name, ICSVLog instance)
        {
            if (Exist(name))
                return false;

            _itemDic.Add(name, instance);
            return true;
        }

        /// <summary>
        /// Retrieves an existing <see cref="ICSVLog"/> instance by name.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <returns>The requested <see cref="ICSVLog"/> instance, or <c>null</c> if not found.</returns>
        internal static ICSVLog? Get(string name)
        {
            _itemDicMutex.WaitOne();
            var result = _itemDic.ContainsKey(name) ? _itemDic[name] : null;
            _itemDicMutex.ReleaseMutex();
            return result;
        }

        /// <summary>
        /// Retrieves a list of all registered log names.
        /// </summary>
        /// <returns>A list of log names currently managed.</returns>
        internal static List<string> GetItemListName()
        {
            List<string> resultList;
            _itemDicMutex.WaitOne();
            resultList = new List<string>(_itemDic.Keys);
            _itemDicMutex.ReleaseMutex();
            return resultList;
        }

        /// <summary>
        /// Checks if a log with the specified name exists.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <returns><c>true</c> if the log exists, <c>false</c> otherwise.</returns>
        private static bool Exist(string name)
        {
            _itemDicMutex.WaitOne();
            bool result = _itemDic.ContainsKey(name);
            _itemDicMutex.ReleaseMutex();
            return result;
        }


    }
}
