using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Threading;

namespace SimpleFileIO.Log.Csv
{
    internal static class CSVLogManager
    {
        private static Dictionary<string, ICSVLog> _itemDic = new();
        private static Mutex _itemDicMutex = new();

        internal static ICSVLog? Create(string name, PathProperty properties)
        {
            if (name is null)
                return null;

            if (Exist(name))
                return Get(name);

            CSVLog_BaseForm addItem = new CSVLog_BaseForm();
            addItem.Property = properties;
            _itemDic.Add(name, addItem);
            return Get(name);
        }

        internal static bool Add(string name, ICSVLog instance)
        {
            if (Exist(name))
                return false;

            _itemDic.Add(name, instance);
            return true;
        }

        internal static ICSVLog? Get(string name)
        {
            _itemDicMutex.WaitOne();
            var result = _itemDic.ContainsKey(name) ? _itemDic[name] : null;
            _itemDicMutex.ReleaseMutex();
            return result;
        }

        internal static List<string> GetItemListName()
        {
            List<string> resultList;
            _itemDicMutex.WaitOne();
            resultList = new List<string>(_itemDic.Keys);
            _itemDicMutex.ReleaseMutex();
            return resultList;
        }

        private static bool Exist(string name)
        {
            _itemDicMutex.WaitOne();
            bool result = _itemDic.ContainsKey(name);
            _itemDicMutex.ReleaseMutex();
            return result;
        }


    }
}
