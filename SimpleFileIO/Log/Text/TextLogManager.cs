using SimpleFileIO.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleFileIO.Log.Text
{
    internal static class TextLogManager
    {
        private static Dictionary<string, ITextLog> _itemDic = new();
        private static Mutex _itemDicMutex = new();

        internal static ITextLog? Create(string name, PathProperty properties)
        {
            if (name is null)
                return null;
            if (Exist(name) is true)
                return Get(name);
            TextLog_BaseForm addItem = new TextLog_BaseForm();
            addItem.Properties = properties;
            _itemDic.Add(name, addItem);
            return Get(name);
        }
        internal static bool Add(string name, ITextLog instance)
        {
            if (Exist(name) is true)
                return false;
            _itemDic.Add(name, instance);
            return true;
        }

        internal static ITextLog? Get(string name)
        {
            if (Exist(name) is false)
                return null;
            _itemDicMutex.WaitOne();
            ITextLog tempItem = _itemDic[name];
            _itemDicMutex.ReleaseMutex();
            return tempItem;

        }

        internal static List<string> GetItemListName()
        {
            List<string> resultList = [];
            _itemDicMutex.WaitOne();
            resultList = new(_itemDic.Keys.ToList());
            _itemDicMutex.ReleaseMutex();
            return resultList;
        }

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
