using System;

namespace SimpleFileIO.Utility
{
    public struct StringTypeParser
    {
        public Type TargetType;
        public Func<string, object?> StringToObject;
        public Func<object, string?> ObjectToString;
    }
}
