using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleFileIO.State.Ini
{
    /// <summary>
    /// Manages multiple instances of INI-based states in a thread-safe manner.
    /// This static class provides functionality for creating, retrieving, and managing INI state instances.
    /// </summary>
    public static class INIStateManager
    {
        /// <summary>
        /// Stores instances of <see cref="IINIState"/> using a dictionary with unique names as keys.
        /// </summary>
        private static Dictionary<string, IINIState> _itemDic = new();

        /// <summary>
        /// Ensures thread safety when accessing or modifying <see cref="_itemDic"/>.
        /// </summary>
        private static Mutex _itemDicMutex = new();

        /// <summary>
        /// Creates a new INI state instance with the specified name and properties.
        /// If a state with the same name already exists, it returns the existing instance.
        /// </summary>
        /// <param name="name">Unique name of the state.</param>
        /// <param name="properties">Path properties of the state file.</param>
        /// <returns>The created or existing <see cref="IINIState"/> instance, or <c>null</c> if name is invalid.</returns>
        internal static IINIState? Create(string name, PathProperty properties)
        {
            if (name is null)
                return null;
            if (Exist(name) is true)
                return Get(name);
            INIState_BaseForm addItem = new INIState_BaseForm();
            addItem.PathProperty = properties;
            _itemDic.Add(name, addItem);
            return Get(name);
        }

        /// <summary>
        /// Adds an existing <see cref="IINIState"/> instance to the manager.
        /// </summary>
        /// <param name="name">Unique name of the state.</param>
        /// <param name="instance">Instance of <see cref="IINIState"/>.</param>
        /// <returns><c>true</c> if added successfully, <c>false</c> if a state with the same name already exists.</returns>
        internal static bool Add(string name, IINIState instance)
        {
            if (Exist(name) is true)
                return false;
            _itemDic.Add(name, instance);
            return true;
        }

        /// <summary>
        /// Retrieves an existing <see cref="IINIState"/> instance by name.
        /// </summary>
        /// <param name="name">Unique name of the state.</param>
        /// <returns>The requested <see cref="IINIState"/> instance, or <c>null</c> if not found.</returns>
        internal static IINIState? Get(string name)
        {
            if (Exist(name) is false)
                return null;
            _itemDicMutex.WaitOne();
            IINIState tempLogger = _itemDic[name];
            _itemDicMutex.ReleaseMutex();
            return tempLogger;

        }

        /// <summary>
        /// Retrieves a list of all registered state names.
        /// </summary>
        /// <returns>A list of state names currently managed.</returns>
        internal static List<string> GetItemListName()
        {
            List<string> resultList = [];
            _itemDicMutex.WaitOne();
            resultList = new(_itemDic.Keys.ToList());
            _itemDicMutex.ReleaseMutex();
            return resultList;
        }

        /// <summary>
        /// Checks if a state with the specified name exists.
        /// </summary>
        /// <param name="name">Unique name of the state.</param>
        /// <returns><c>true</c> if the state exists, <c>false</c> otherwise.</returns>
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
