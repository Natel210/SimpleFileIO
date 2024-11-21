using SimpleFileIO.Log.Csv;
using SimpleFileIO.Log.Text;
using SimpleFileIO.State.Ini;
using SimpleFileIO.Utility;
using System.Linq;

namespace SimpleFileIO
{
    public static class Manager
    {
        #region TextLog
        
        public static ITextLog? CreateTextLog(string name, PathProperty properties)
        {
            return TextLogManager.Create(name, properties);
        }

        public static bool AddTextLog(string name, ITextLog instance)
        {
            return TextLogManager.Add(name, instance);
        }

        public static ITextLog? GetTextLog(string name)
        {
            return TextLogManager.Get(name);
        }

        public static List<string> GetTextLogListName()
        {
            return TextLogManager.GetItemListName();
        }

        #endregion

        #region CSVLog

        public static ICSVLog? CreateCsvLog(string name, PathProperty properties)
        {
            return CSVLogManager.Create(name, properties);
        }

        public static bool AddCsvLog(string name, ICSVLog instance)
        {
            return CSVLogManager.Add(name, instance);
        }

        public static ICSVLog? GetCsvLog(string name)
        {
            return CSVLogManager.Get(name);
        }

        public static List<string> GetCsvLogListName()
        {
            return CSVLogManager.GetItemListName();
        }

        #endregion

        #region INIState

        public static IINIState? CreateIniState(string name, PathProperty properties)
        {
            return INIStateManager.Create(name, properties);
        }

        public static bool AddIniState(string name, IINIState instance)
        {
            return INIStateManager.Add(name, instance);
        }

        public static IINIState? GetIniState(string name)
        {
            return INIStateManager.Get(name);
        }

        public static List<string> GetIniStateListName()
        {
            return INIStateManager.GetItemListName();
        }

        #endregion

        static Manager()
        {
            //Create(_defaultLog);
        }


    }
}
