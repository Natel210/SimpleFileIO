using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SimpleFileIO.Resources.ErrorMessages;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SimpleFileIO.Log.Csv
{
    /// <summary>
    /// Provides a type converter for converting arrays of type <typeparamref name="T"/> to and from CSV format.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    public class CSVLogArrayConverter<T> : ITypeConverter
    {
        /// <summary>
        /// Converts an object to a CSV-formatted string.
        /// </summary>
        /// <param name="value">The object to be converted.</param>
        /// <param name="row">The CSV writer row.</param>
        /// <param name="memberMapData">Metadata about the member being mapped.</param>
        /// <returns>A CSV-formatted string representation of the array.</returns>
        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return string.Empty;
            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_type}, Type '{typeof(T)}'");
            var array = value as T[];
            if (array == null)
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_array_type}, Type '{typeof(T)}'");
            return string.Join(";", array.Select(item => item?.ToString()));
        }

        /// <summary>
        /// Converts a CSV-formatted string to an array of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="text">The CSV string.</param>
        /// <param name="row">The CSV reader row.</param>
        /// <param name="memberMapData">Metadata about the member being mapped.</param>
        /// <returns>An array of type <typeparamref name="T"/>.</returns>
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<T>();

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_type}, Type '{typeof(T)}'");

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
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_type}, Type '{typeof(T)}'");
        }

        /// <summary>
        /// Determines whether the specified type is supported for conversion.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is supported; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Provides a type converter for converting lists of type <typeparamref name="T"/> to and from CSV format.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class CSVLogListConverter<T> : ITypeConverter
    {
        /// <summary>
        /// Converts an object to a CSV-formatted string.
        /// </summary>
        /// <param name="value">The object to be converted.</param>
        /// <param name="row">The CSV writer row.</param>
        /// <param name="memberMapData">Metadata about the member being mapped.</param>
        /// <returns>A CSV-formatted string representation of the list.</returns>
        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return string.Empty;

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_type}, Type '{typeof(T)}'");

            var list = value as List<T>;
            if (list == null)
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_list_type}, Type '{typeof(T)}'");

            return string.Join(";", list.Select(item => item?.ToString()));
        }

        /// <summary>
        /// Converts a CSV-formatted string to a list of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="text">The CSV string.</param>
        /// <param name="row">The CSV reader row.</param>
        /// <param name="memberMapData">Metadata about the member being mapped.</param>
        /// <returns>A list of type <typeparamref name="T"/>.</returns>
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return new List<T>();

            if (!IsValidType(typeof(T)))
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_type}, Type '{typeof(T)}'");

            var elements = text.Split(';');

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
                throw new InvalidOperationException($"{ErrorMessages.csv_log_array_converter_invalid_type}, Type '{typeof(T)}'");
        }

        /// <summary>
        /// Determines whether the specified type is supported for conversion.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is supported; otherwise, <c>false</c>.</returns>
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