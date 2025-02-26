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
            textLog.ThrowExceptionMode = true;
            textLog.Add("aaa");
            textLog.Add("bbb");
            textLog.Write();
            Manager.CreateCsvLog("TestCSVLog", new() { RootDirectory = new("./Test"), FileName = "TestCSVLog", Extension = "csvlog" });
            ICSVLog? csvLog = Manager.GetCsvLog("TestCSVLog") ?? null;
            if (csvLog is null)
                throw new Exception("not create csv log.");
            var data2 = new testdata2();
            csvLog.ThrowExceptionMode = true;
            csvLog.Add(data2);
            csvLog.Add(data2);
            csvLog.Write();
            List<testdata2> temp1;
            using (var reader = new StreamReader("./Test/TestCSVLog.csvlog"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                temp1 = csv.GetRecords<testdata2>().ToList();

            Manager.CreateIniState("TestIni", new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = "ini" });
            IINIState? iniState = Manager.GetIniState("TestIni") ?? null;
            if (iniState is null)
                throw new Exception("not create ini.");
            iniState.ThrowExceptionMode = true;
            
            //Edit item content 
            iniState.SetValue("MY", "ITEM1", "ABC");
            iniState.SetValue("MY", "ITEM2", "DEF");
            iniState.SetValue("YOUR", "CAR1", "GHI");
            iniState.SetValue("YOUR", "CAR2", "JKL");
            //Edit item content (using parser)
            iniState.SetValue_UseParser<byte>("Parser", "byte", (byte)1);
            iniState.SetValue_UseParser<sbyte>("Parser", "sbyte", (sbyte)2);
            iniState.SetValue_UseParser<short>("Parser", "short", (short)3);
            iniState.SetValue_UseParser<ushort>("Parser", "ushort", (ushort)4);
            iniState.SetValue_UseParser<int>("Parser", "int", (int)5);
            iniState.SetValue_UseParser<uint>("Parser", "uint", (uint)6);
            iniState.SetValue_UseParser<long>("Parser", "long", (long)7);
            iniState.SetValue_UseParser<ulong>("Parser", "ulong", (ulong)8);
            iniState.SetValue_UseParser<float>("Parser", "float", (float)9.9f);
            iniState.SetValue_UseParser<double>("Parser", "double", (double)10.10);
            iniState.SetValue_UseParser<decimal>("Parser", "decimal", (decimal)11.11);
            iniState.SetValue_UseParser<char>("Parser", "char", (char)'a');
            iniState.SetValue_UseParser<string>("Parser", "string", (string)"b");
            iniState.SetValue_UseParser<bool>("Parser", "bool", (bool)true);
            iniState.SetValue_UseParser<string[]>("Parser", "string array", new[] { "c", "d", "e", "f" });
            iniState.SetValue_UseParser<PathProperty>("Parser", "path property",
                new PathProperty{ RootDirectory = new DirectoryInfo("./GGGG"), FileName = "HHHH", Extension = "IIII" });
            //Save
            iniState.Save();


            // new load and check datas
            IINIState? findINI = Manager.CreateIniState("TestFindIni",
                new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = "ini" });
            if (findINI is null)
                throw new Exception("not create ini.");
            findINI.ThrowExceptionMode = true;

            findINI.Load();
            findINI.GetValue("MY", "ITEM1", "111");
            findINI.GetValue("MY", "ITEM2", "222");
            findINI.GetValue("YOUR", "CAR1", "333");
            findINI.GetValue("YOUR", "CAR2", "444");
            //Edit item content (using parser)
            var getByte = findINI.GetValue_UseParser<byte>("Parser", "byte", (byte)0);
            var getSbyte = findINI.GetValue_UseParser<sbyte>("Parser", "sbyte", (sbyte)0);
            var getShort = findINI.GetValue_UseParser<short>("Parser", "short", (short)0);
            var getUshort = findINI.GetValue_UseParser<ushort>("Parser", "ushort", (ushort)0);
            var getInt = findINI.GetValue_UseParser<int>("Parser", "int", (int)0);
            var getUint = findINI.GetValue_UseParser<uint>("Parser", "uint", (uint)0);
            var getLong = findINI.GetValue_UseParser<long>("Parser", "long", (long)0);
            var getUlong = findINI.GetValue_UseParser<ulong>("Parser", "ulong", (ulong)0);
            var getFloat = findINI.GetValue_UseParser<float>("Parser", "float", (float)0.0f);
            var getDouble = findINI.GetValue_UseParser<double>("Parser", "double", (double)0.0);
            var getDecimal = findINI.GetValue_UseParser<decimal>("Parser", "decimal", (decimal)0.0);
            var getChar = findINI.GetValue_UseParser<char>("Parser", "char", (char)'x');
            var getString = findINI.GetValue_UseParser<string>("Parser", "string", (string)"x");
            var getBool = findINI.GetValue_UseParser<bool>("Parser", "bool", (bool)true);
            var getStringArray = findINI.GetValue_UseParser<string[]>("Parser", "string array", new[] { "x", "x", "x", "x" });
            var getPathProperty = findINI.GetValue_UseParser<PathProperty>("Parser", "path property",
                new PathProperty { RootDirectory = new DirectoryInfo("./XXXX"), FileName = "XXXX", Extension = "XXXX" });

            //out puts
            Console.WriteLine(getByte);
            Console.WriteLine(getSbyte);
            Console.WriteLine(getShort);
            Console.WriteLine(getUshort);
            Console.WriteLine(getInt);
            Console.WriteLine(getUint);
            Console.WriteLine(getLong);
            Console.WriteLine(getUlong);
            Console.WriteLine(getFloat);
            Console.WriteLine(getDouble);
            Console.WriteLine(getDecimal);
            Console.WriteLine(getChar);
            Console.WriteLine(getString);
            Console.WriteLine(getBool);
            Console.WriteLine(string.Join(",", getStringArray));
            Console.WriteLine($"{getPathProperty.RootDirectory}/{getPathProperty.FileName}.{getPathProperty.Extension}");

            // add parser
            IINIState? parserINI = Manager.CreateIniState("TestparserIni",
                new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = ".ini" });
            if (parserINI is null)
                throw new Exception("not create ini.");
            parserINI.ThrowExceptionMode = true;

            var stringTypeParser = new StringTypeParser() {
                TargetType = typeof(parserTpye1),
                ObjectToString = (obj) =>
                {
                    if (obj is parserTpye1 item)
                        return $"{item.Name},{item.Index}";
                    return null;
                },
                StringToObject = (str) =>
                {
                    var result = new parserTpye1();
                    var strArray = str.Split(',');
                    if (strArray.Count() is 2)
                    {
                        result.Name = strArray[0];
                        result.Index = Int32.Parse(strArray[1]);
                    }
                    return result;
                },
            };
            parserINI.AddParser(typeof(parserTpye1), stringTypeParser);

            var tempItem = new parserTpye1() { Name = "MYNAME", Index = 10 };
            parserINI.SetValue_UseParser<parserTpye1>("Add Parser", "Parser Tpye1", tempItem);
            var getItem = parserINI.GetValue_UseParser<parserTpye1>("Add Parser", "Parser Tpye1", tempItem);

        }
    }
}
