using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleFileIO.Log.Text
{
    /// <summary>
    /// Manages multiple instances of text-based logs in a thread-safe manner.
    /// This static class provides functionality for creating, retrieving, and managing text log instances.
    /// </summary>
    internal static class TextLogManager
    {
        /// <summary>
        /// Stores instances of <see cref="ITextLog"/> using a dictionary with unique names as keys.
        /// </summary>
        private static Dictionary<string, ITextLog> _itemDic = new();

        /// <summary>
        /// Ensures thread safety when accessing or modifying <see cref="_itemDic"/>.
        /// </summary>
        private static Mutex _itemDicMutex = new();

        /// <summary>
        /// Creates a new text log instance with the specified name and properties.
        /// If a log with the same name already exists, it returns the existing instance.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="properties">Path properties of the log file.</param>
        /// <returns>The created or existing <see cref="ITextLog"/> instance, or <c>null</c> if name is invalid.</returns>
        internal static ITextLog? Create(string name, PathProperty properties)
        {
            if (name is null)
                return null;
            if (Exist(name) is true)
                return Get(name);
            TextLog_BaseForm addItem = new TextLog_BaseForm();
            addItem.PathProperty = properties;
            _itemDic.Add(name, addItem);
            return Get(name);
        }

        /// <summary>
        /// Adds an existing <see cref="ITextLog"/> instance to the manager.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <param name="instance">Instance of <see cref="ITextLog"/>.</param>
        /// <returns><c>true</c> if added successfully, <c>false</c> if a log with the same name already exists.</returns>
        internal static bool Add(string name, ITextLog instance)
        {
            if (Exist(name) is true)
                return false;
            _itemDic.Add(name, instance);
            return true;
        }

        /// <summary>
        /// Retrieves an existing <see cref="ITextLog"/> instance by name.
        /// </summary>
        /// <param name="name">Unique name of the log.</param>
        /// <returns>The requested <see cref="ITextLog"/> instance, or <c>null</c> if not found.</returns>
        internal static ITextLog? Get(string name)
        {
            if (Exist(name) is false)
                return null;
            _itemDicMutex.WaitOne();
            ITextLog tempItem = _itemDic[name];
            _itemDicMutex.ReleaseMutex();
            return tempItem;

        }

        /// <summary>
        /// Retrieves a list of all registered log names.
        /// </summary>
        /// <returns>A list of log names currently managed.</returns>
        internal static List<string> GetItemListName()
        {
            List<string> resultList = [];
            _itemDicMutex.WaitOne();
            resultList = new(_itemDic.Keys.ToList());
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
            bool result = false;
            _itemDicMutex.WaitOne();
            result = _itemDic.ContainsKey(name);
            _itemDicMutex.ReleaseMutex();
            return result;
        }
    }
}
