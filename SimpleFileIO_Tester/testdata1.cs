using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_CONSOLE
{
    internal class testdata1
    {
        [Name("Int1")]
        public int int1 { get; set; } = 1;
        [Name("Int2")]
        public int int2 { get; set; } = 2;
        [Name("Int3")]
        public int int3 { get; set; } = 3;
        [Name("Int4")]
        public int int4 { get; set; } = 4;
        [Name("Flaot1")]
        public float flaot1 { get; set; } = 5.5f;
        [Name("Flaot2")]
        public float flaot2 { get; set; } = 6.6f;
        [Name("Flaot3")]
        public float flaot3 { get; set; } = 7.7f;
        [Name("Flaot4")]
        public float flaot4 { get; set; } = 8.8f;
        [Name("String1")]
        public string string1 { get; set; } = "S,9";
        [Name("String2")]
        public string string2 { get; set; } = "S,10";
        [Name("String3")]
        public string string3 { get; set; } = "S,11";
        [Name("String4")]
        public string string4 { get; set; } = "S,12";

    }
}
