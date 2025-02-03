using CsvHelper;
using SimpleFileIO;
using SimpleFileIO.Log.Csv;
using SimpleFileIO.Log.Text;
using SimpleFileIO.State.Ini;
using SimpleFileIO.Utility;
using System.Globalization;
using System.Resources;
using TEST_CONSOLE;

namespace SimpleFileIO_Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Manager.CreateTextLog("TestTextLog",new() { RootDirectory = new("./Test"), FileName="TestTextLog", Extension="txtlog" });
            ITextLog? textLog = Manager.GetTextLog("TestTextLog") ?? null;
            if (textLog is null)
                throw new Exception("not create text log.");
            textLog.Add("aaa");
            textLog.Add("bbb");
            textLog.Write();
            Manager.CreateCsvLog("TestCSVLog", new() { RootDirectory = new("./Test"), FileName = "TestCSVLog", Extension = "csvlog" });
            ICSVLog? csvLog = Manager.GetCsvLog("TestCSVLog") ?? null;
            if (csvLog is null)
                throw new Exception("not create csv log.");
            var data2 = new testdata2();
            csvLog.Add(data2);
            csvLog.Add(data2);
            csvLog.Write();
            List<testdata2> temp1;
            using (var reader = new StreamReader("./Test/TestCSVLog.csvlog"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                temp1 = csv.GetRecords<testdata2>().ToList();

            Manager.CreateIniState("TestIni", new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = ".ini" });
            IINIState? iniState = Manager.GetIniState("TestIni") ?? null;
            if (iniState is null)
                throw new Exception("not create ini.");
            iniState.Load();
            iniState.SetValue("MY", "ITEM1", "111");
            iniState.SetValue("MY", "ITEM2", "222");
            iniState.SetValue("YOUR", "CAR1", "333");
            iniState.SetValue("YOUR", "CAR2", "444");
            iniState.Save();
            iniState.Load();

            ////
        }
    }
}
