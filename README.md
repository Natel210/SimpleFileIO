# SimpleFileIO

## Summary

SimpleFileIO is a library designed to provide a unified interface for record-based IO management. It supports the following file types and features:

- CSV: Structured data storage in CSV format.
- Appendable CSV (Log): Incremental logging directly into a CSV file.
- Text: General text file management.
- Appendable Text (Log): Incremental logging into plain text files.

This library eliminates unnecessary dependencies while maintaining flexibility and ease of use. CsvHelper is used for advanced CSV management.

## Key Features

- Unified interface for multiple file types (CSV, Text, Logs).
- Modular approach.

## How to

### Text Log
```cs
using SimpleFileIO;
using SimpleFileIO.Utility;
using SimpleFileIO.Log.Text;

// Create Intance
PathProperty pathProperty = new PathProperty() {
    RootDirectory = new DirectoryInfo("./Test"),
    FileName="TestTextLog",
    Extension="txtlog" };
ITextLog? returnItem = Manager.CreateTextLog("TestTextLog", pathProperty);
// Get Intance
ITextLog? getItem = Manager.GetTextLog("TestTextLog") ?? null;
// Check Invaild Intance
if (getItem is null)
    throw new Exception("not create text log.");
// Add log contents
getItem.Add("aaa");
getItem.Add("bbb");
// Write
getItem.Write();
```
### CSV Log
```cs
using CsvHelper;
using SimpleFileIO;
using SimpleFileIO.Utility;
using SimpleFileIO.Log.Csv;

// Create Intance
PathProperty pathProperty = new PathProperty() {
    RootDirectory = new DirectoryInfo("./Test"),
    FileName="TestCSVLog",
    Extension="csvlog" };
ICSVLog? returnItem = Manager.CreateCsvLog("TestCSVLog", pathProperty);
// Get Intance
ICSVLog? getItem = Manager.GetCsvLog("TestCSVLog") ?? null;
// Check Invaild Intance
if (getItem is null)
    throw new Exception("not create text log.");
// Create log Item
var data1 = new testdata();
data1.int1 = 01;
var data2 = new testdata();
data1.int2 = 02;

// Add log contents
getItem.Add(data1);
getItem.Add(data2);
// Write
getItem.Write();
```
```cs
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace YourNamespace
{
    internal class testdata
    {
        [Name("Int1")]
        public int int1 { get; set; } = 21;
        [Name("Int2")]
        public int int2 { get; set; } = 22;
        [Name("Int3")]
        public int int3 { get; set; } = 23;
        [Name("Flaot1")]
        public float flaot1 { get; set; } = 25.5f;
        [Name("Flaot2")]
        public float flaot2 { get; set; } = 26.6f;
        [Name("Flaot3")]
        public float flaot3 { get; set; } = 27.7f;
        [Name("String1")]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(StringArrayConverter))]
        public string[] string1 { get; set; } = { "2S,9", "43424", "23443" };
        [Name("String2")]
        public string string2 { get; set; } = "2S,10";
        [Name("String3")]
        public string string3 { get; set; } = "2S,11";
    }
}

//Use Custom
public class StringArrayConverter : ITypeConverter
{
    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value == null)
        {
            return string.Empty;
        }
        // to linearization, separator character ';'
        return string.Join(";", (string[])value);
    }

    public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return Array.Empty<string>();
        }
        // to array, separator character ';'
        return text.Split(';');
    }
}
```
### INI State Save
```cs
using SimpleFileIO;
using SimpleFileIO.Utility;
using SimpleFileIO.State.Ini;

// Create Intance
Manager.CreateIniState("TestIni", new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = ".ini" });
// Get Intance
IINIState? iniState = Manager.GetIniState("TestIni") ?? null;
// Check Invaild Intance
if (iniState is null)
    throw new Exception("not create ini.");
// Load
iniState.Load();
// Edit item contents [Section][Key][Value]
iniState.SetValue("MY", "ITEM1", "111");
iniState.SetValue("MY", "ITEM2", "222");
iniState.SetValue("YOUR", "CAR1", "333");
iniState.SetValue("YOUR", "CAR2", "444");
//Edit item content (using parser) [Section][Key][(type)Value]
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

```

### INI State Load
```cs
using SimpleFileIO;
using SimpleFileIO.Utility;
using SimpleFileIO.State.Ini;
/////////////////////////////////////////////////
// New load and check datas
IINIState? findINI = Manager.CreateIniState("TestFindIni",
    new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = ".ini" });
if (findINI is null)
    throw new Exception("not create ini.");
findINI.ThrowExceptionMode = true;
// load
findINI.Load();
// Get Value [Section][Key][Default]
findINI.GetValue("MY", "ITEM1", "111");
findINI.GetValue("MY", "ITEM2", "222");
findINI.GetValue("YOUR", "CAR1", "333");
findINI.GetValue("YOUR", "CAR2", "444");
// Get Value (using parser) [Section][Key][(type)Default]
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
```

### INI State Add Parser
```cs
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
```
```cs
internal class parserTpye1
{
    internal string Name { get; set; } = "";
    internal int Index { get; set; } = -1;
}
```

## Notes
- 0.1.5 ~ 0.1.6 Remove Temp Ini Dir / Changed Output Dir in Code.
- 0.1.1 ~ 0.1.4 Fix action workflow: Prevent push-triggered reversion when updating README or .yml files.
- 0.0.3 ~ 0.1.1 fill github data & update action workflows
- 0.0.1 ~ 0.0.3 upload nuget
- 0.0.1 Init

## Third-Party Libraries

### CsvHelper
- Version: 33.0.1
- License: Apache-2.0 OR MS-PL
- Copyright: © 2009–2024 Josh Close
- Project URL: [CsvHelper Project](https://joshclose.github.io/CsvHelper)
- Nuget: [CsvHelper NuGet](https://www.nuget.org/packages/CsvHelper)
- Usage:  
  - Used for advanced CSV parsing and serialization.
