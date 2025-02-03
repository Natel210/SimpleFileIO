using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleFileIO.State.Ini
{
    public static class INIStateManager
    {
        private static Dictionary<string, IINIState> _itemDic = new();
        private static Mutex _itemDicMutex = new();

        internal static IINIState? Create(string name, PathProperty properties)
        {
            if (name is null)
                return null;
            if (Exist(name) is true)
                return Get(name);
            INIState_BaseForm addItem = new INIState_BaseForm();
            addItem.Properties = properties;
            _itemDic.Add(name, addItem);
            return Get(name);
        }

        internal static bool Add(string name, IINIState instance)
        {
            if (Exist(name) is true)
                return false;
            _itemDic.Add(name, instance);
            return true;
        }

        internal static IINIState? Get(string logName)
        {
            if (Exist(logName) is false)
                return null;
            _itemDicMutex.WaitOne();
            IINIState tempLogger = _itemDic[logName];
            _itemDicMutex.ReleaseMutex();
            return tempLogger;

        }

        internal static List<string> GetItemListName()
        {
            List<string> resultList = [];
            _itemDicMutex.WaitOne();
            resultList = new(_itemDic.Keys.ToList());
            _itemDicMutex.ReleaseMutex();
            return resultList;
        }

        private static bool Exist(string logName)
        {
            bool result = false;
            _itemDicMutex.WaitOne();
            result = _itemDic.ContainsKey(logName);
            _itemDicMutex.ReleaseMutex();
            return result;
        }
    }
}
