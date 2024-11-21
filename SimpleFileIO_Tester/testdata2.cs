using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace TEST_CONSOLE
{
    internal class testdata2
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
        public string[] string1 { get; set; } = { "2S,9", "43424", "23443" };  // 배열 초기화 수정
        [Name("String2")]
        public string string2 { get; set; } = "2S,10";
        [Name("String3")]
        public string string3 { get; set; } = "2S,11";
    }
}

public class StringArrayConverter : ITypeConverter
{
    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value == null)
        {
            return string.Empty;
        }

        return string.Join(";", (string[])value); // string 배열을 세미콜론으로 구분된 문자열로 변환
    }

    // CSV에서 문자열을 배열로 변환할 때
    public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return Array.Empty<string>();
        }

        return text.Split(';'); // 세미콜론으로 구분된 문자열을 배열로 변환
    }
}