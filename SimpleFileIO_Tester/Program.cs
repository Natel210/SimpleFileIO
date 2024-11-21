using SimpleFileIO;
using SimpleFileIO.Log.Csv;
using SimpleFileIO.Log.Text;
using TEST_CONSOLE;

namespace SimpleFileIO_Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manager.CreateTextLog("TestTextLog",new() { RootDirectory = new("./Test"), FileName="TestTextLog", Extension=".txtlog" });
            ITextLog? textLog = Manager.GetTextLog("TestTextLog") ?? null;
            if (textLog is null)
                throw new Exception("not create text log.");
            textLog.Add("aaa");
            textLog.Add("bbb");
            textLog.Write();
            Manager.CreateCsvLog("TestCSVLog", new() { RootDirectory = new("./Test"), FileName = "TestCSVLog", Extension = ".csvlog" });
            ICSVLog? csvLog = Manager.GetCsvLog("TestCSVLog") ?? null;
            if (csvLog is null)
                throw new Exception("not create csv log.");
            var data2 = new testdata2();
            csvLog.Add(data2);
            csvLog.Add(data2);
            csvLog.Write();

            Manager.CreateIniState("TestIni", new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = ".ini" });





            Console.WriteLine("Hello, World!");
        }
    }
}
