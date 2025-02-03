using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SimpleFileIO.Log.Csv
{
    public class CSVLogArrayConverter<T> : ITypeConverter
    {
        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return string.Empty;

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"Type '{typeof(T)}' is not supported for conversion.");

            var array = value as T[];
            if (array == null)
                throw new InvalidOperationException("The value is not an array of the specified type.");

            return string.Join(";", array.Select(item => item?.ToString()));
        }

        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<T>();

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"Type '{typeof(T)}' is not supported for conversion.");

            var elements = text.Split(';');
            if (typeof(T) == typeof(bool))
                return elements.Select(e => bool.Parse(e)).ToArray();
            else if (typeof(T) == typeof(char))
                return elements.Select(e => char.Parse(e)).ToArray();
            else if (typeof(T) == typeof(string))
                return elements.ToArray();
            else if (typeof(T) == typeof(byte))
                return elements.Select(e => byte.Parse(e)).ToArray();
            else if (typeof(T) == typeof(sbyte))
                return elements.Select(e => sbyte.Parse(e)).ToArray();
            else if (typeof(T) == typeof(short))
                return elements.Select(e => short.Parse(e)).ToArray();
            else if (typeof(T) == typeof(ushort))
                return elements.Select(e => ushort.Parse(e)).ToArray();
            else if (typeof(T) == typeof(double))
                return elements.Select(e => double.Parse(e)).ToArray();
            else if (typeof(T) == typeof(float))
                return elements.Select(e => float.Parse(e)).ToArray();
            else if (typeof(T) == typeof(decimal))
                return elements.Select(e => decimal.Parse(e)).ToArray();
            else if (typeof(T) == typeof(int))
                return elements.Select(e => int.Parse(e)).ToArray();
            else if (typeof(T) == typeof(uint))
                return elements.Select(e => uint.Parse(e)).ToArray();
            else if (typeof(T) == typeof(nint))
                return elements.Select(e => nint.Parse(e)).ToArray();
            else if (typeof(T) == typeof(nuint))
                return elements.Select(e => nuint.Parse(e)).ToArray();
            else if (typeof(T) == typeof(long))
                return elements.Select(e => long.Parse(e)).ToArray();
            else if (typeof(T) == typeof(ulong))
                return elements.Select(e => ulong.Parse(e)).ToArray();
            else
                throw new InvalidOperationException($"Type '{typeof(T)}' is not supported for conversion.");
        }

        private bool IsValidType(Type type)
        {
            return type == typeof(bool)
                   || type == typeof(char) || type == typeof(string)
                   || type == typeof(byte) || type == typeof(sbyte)
                   || type == typeof(short) || type == typeof(ushort)
                   || type == typeof(double) || type == typeof(float) || type == typeof(decimal)
                   || type == typeof(int) || type == typeof(uint)
                   || type == typeof(nint) || type == typeof(nuint)
                   || type == typeof(long) || type == typeof(ulong);
        }
    }

    public class CSVLogListConverter<T> : ITypeConverter
    {
        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return string.Empty;

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"Type '{typeof(T)}' is not supported for conversion.");

            var list = value as List<T>;
            if (list == null)
                throw new InvalidOperationException("The value is not a List of the specified type.");

            return string.Join(";", list.Select(item => item?.ToString()));
        }

        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return new List<T>();

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"Type '{typeof(T)}' is not supported for conversion.");

            var elements = text.Split(';');

            // 각 타입에 맞게 변환
            if (typeof(T) == typeof(bool))
                return elements.Select(e => bool.Parse(e)).ToList();
            else if (typeof(T) == typeof(char))
                return elements.Select(e => char.Parse(e)).ToList();
            else if (typeof(T) == typeof(string))
                return elements.ToList();
            else if (typeof(T) == typeof(byte))
                return elements.Select(e => byte.Parse(e)).ToList();
            else if (typeof(T) == typeof(sbyte))
                return elements.Select(e => sbyte.Parse(e)).ToList();
            else if (typeof(T) == typeof(short))
                return elements.Select(e => short.Parse(e)).ToList();
            else if (typeof(T) == typeof(ushort))
                return elements.Select(e => ushort.Parse(e)).ToList();
            else if (typeof(T) == typeof(double))
                return elements.Select(e => double.Parse(e)).ToList();
            else if (typeof(T) == typeof(float))
                return elements.Select(e => float.Parse(e)).ToList();
            else if (typeof(T) == typeof(decimal))
                return elements.Select(e => decimal.Parse(e)).ToList();
            else if (typeof(T) == typeof(int))
                return elements.Select(e => int.Parse(e)).ToList();
            else if (typeof(T) == typeof(uint))
                return elements.Select(e => uint.Parse(e)).ToList();
            else if (typeof(T) == typeof(nint))
                return elements.Select(e => nint.Parse(e)).ToList();
            else if (typeof(T) == typeof(nuint))
                return elements.Select(e => nuint.Parse(e)).ToList();
            else if (typeof(T) == typeof(long))
                return elements.Select(e => long.Parse(e)).ToList();
            else if (typeof(T) == typeof(ulong))
                return elements.Select(e => ulong.Parse(e)).ToList();
            else
                throw new InvalidOperationException($"Type '{typeof(T)}' is not supported for conversion.");
        }

        // 기본 자료형만 허용하는 메서드
        private bool IsValidType(Type type)
        {
            return type == typeof(bool)
                   || type == typeof(char) || type == typeof(string)
                   || type == typeof(byte) || type == typeof(sbyte)
                   || type == typeof(short) || type == typeof(ushort)
                   || type == typeof(double) || type == typeof(float) || type == typeof(decimal)
                   || type == typeof(int) || type == typeof(uint)
                   || type == typeof(nint) || type == typeof(nuint)
                   || type == typeof(long) || type == typeof(ulong);
        }
    }
}