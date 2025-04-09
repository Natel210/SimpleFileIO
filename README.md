# SimpleFileIO

## 🔍 Summary
SimpleFileIO provides a unified and flexible interface for record-based file I/O operations in .NET applications.
It supports multiple file types including CSV, plain text, and INI for logging or state management, while keeping dependencies minimal.


## ✨ Key Features
- Unified interface for managing CSV, text, and log files
- Support for both read/write and appendable operations
- INI-style structured key-value state handling
- Extendable parser support for custom types
- Lightweight and dependency-aware design


## 🚀 Getting Started

### 1. Write Text Log
```csharp
using SimpleFileIO;
using SimpleFileIO.Utility;
using SimpleFileIO.Log.Text;

// Create instance
PathProperty pathProperty = new() {
    RootDirectory = new DirectoryInfo("./Test"),
    FileName = "TestTextLog",
    Extension = "txtlog"
};
ITextLog? textLog = Manager.CreateTextLog("TestTextLog", pathProperty);

// Retrieve instance
ITextLog? getLog = Manager.GetTextLog("TestTextLog");
if (getLog is null)
    throw new Exception("Text log not found.");

// Append content
getLog.Add("aaa");
getLog.Add("bbb");

// Write to disk
getLog.Write();
```

### 2. Write CSV Log
```csharp
using CsvHelper;
using SimpleFileIO;
using SimpleFileIO.Utility;
using SimpleFileIO.Log.Csv;

// Create instance
PathProperty pathProperty = new() {
    RootDirectory = new DirectoryInfo("./Test"),
    FileName = "TestCSVLog",
    Extension = "csvlog"
};
ICSVLog? csvLog = Manager.CreateCsvLog("TestCSVLog", pathProperty);

// Retrieve instance
ICSVLog? getCsv = Manager.GetCsvLog("TestCSVLog");
if (getCsv is null)
    throw new Exception("CSV log not found.");

// Add data rows
var data1 = new testdata { int1 = 1 };
var data2 = new testdata { int2 = 2 };
csvLog.Add(data1);
csvLog.Add(data2);

// Write to disk
csvLog.Write();
```

```csharp
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration.Attributes;

internal class testdata
{
    [Name("Int1")] public int int1 { get; set; } = 21;
    [Name("Int2")] public int int2 { get; set; } = 22;
    [Name("Int3")] public int int3 { get; set; } = 23;
    [Name("Flaot1")] public float flaot1 { get; set; } = 25.5f;
    [Name("Flaot2")] public float flaot2 { get; set; } = 26.6f;
    [Name("Flaot3")] public float flaot3 { get; set; } = 27.7f;
    [Name("String1"), TypeConverter(typeof(StringArrayConverter))] public string[] string1 { get; set; } = { "2S,9", "43424", "23443" };
    [Name("String2")] public string string2 { get; set; } = "2S,10";
    [Name("String3")] public string string3 { get; set; } = "2S,11";
}

public class StringArrayConverter : ITypeConverter
{
    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        => value == null ? string.Empty : string.Join(";", (string[])value);

    public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        => string.IsNullOrEmpty(text) ? Array.Empty<string>() : text.Split(';');
}
```

### 3. Save INI State
```csharp
using SimpleFileIO.State.Ini;

Manager.CreateIniState("TestIni", new() {
    RootDirectory = new("./Test"),
    FileName = "TestIni",
    Extension = ".ini"
});

IINIState? iniState = Manager.GetIniState("TestIni");
if (iniState is null)
    throw new Exception("INI state not created.");

iniState.Load();
iniState.SetValue("MY", "ITEM1", "111");
iniState.SetValue("YOUR", "CAR1", "333");
iniState.SetValue_UseParser<int>("Parser", "int", 5);
```

### 4. Load INI State
```csharp
IINIState? ini = Manager.CreateIniState("TestFindIni",
    new() { RootDirectory = new("./Test"), FileName = "TestIni", Extension = ".ini" });
if (ini is null)
    throw new Exception("INI not loaded.");
ini.ThrowExceptionMode = true;
ini.Load();

var intVal = ini.GetValue_UseParser<int>("Parser", "int", 0);
var strArray = ini.GetValue_UseParser<string[]>("Parser", "string array", new[] { "a", "b" });
```

### 5. Add Custom INI Parser
```csharp
var customParser = new StringTypeParser()
{
    TargetType = typeof(CustomType),
    ObjectToString = obj => obj is CustomType t ? $"{t.Name},{t.Index}" : null,
    StringToObject = str =>
    {
        var parts = str.Split(',');
        return parts.Length == 2 ? new CustomType { Name = parts[0], Index = int.Parse(parts[1]) } : null;
    }
};
parserINI.AddParser(typeof(CustomType), customParser);

parserINI.SetValue_UseParser("Custom", "Item", new CustomType { Name = "Example", Index = 10 });
var restored = parserINI.GetValue_UseParser("Custom", "Item", new CustomType());

internal class CustomType
{
    public string Name { get; set; } = "";
    public int Index { get; set; } = -1;
}
```


## 📝 Release Notes
- 0.1.6: Refactored output directory logic
- 0.1.5: Removed temporary INI directory usage
- 0.1.1–0.1.4: Updated GitHub Actions, improved README & CI behavior
- 0.0.3–0.1.1: Initial uploads to NuGet
- 0.0.1: Project initialization


## 📦 Third-Party Libraries

### CsvHelper
- Version: 33.0.1
- License: Apache-2.0 OR MS-PL
- Copyright: © 2009–2024 Josh Close
- Project URL: [CsvHelper Project](https://joshclose.github.io/CsvHelper)
- Nuget: [CsvHelper NuGet](https://www.nuget.org/packages/CsvHelper)
- Usage:  
  - Used for advanced CSV parsing and serialization.
