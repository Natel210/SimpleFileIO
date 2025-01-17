﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleFileIO.Utility
{
    public class IniItem<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T> where T : notnull
    {
        public string Section { get; set; } = "";
        public string Key { get; set; } = "";

        public T DefaultValue { get; set; } = CreateInstanceOrAssign();
        public T Value { get; set; } = CreateInstanceOrAssign();

        public bool IsValidSectionKey => !string.IsNullOrWhiteSpace(Section) && !string.IsNullOrWhiteSpace(Key);

        public IniItem()
        {
        }

        public IniItem(IniItem<T> iniItem)
        {
            Section = iniItem.Section ?? throw new ArgumentNullException(nameof(iniItem.Section));
            Key = iniItem.Key ?? throw new ArgumentNullException(nameof(iniItem.Key));
            DefaultValue = CreateInstanceOrAssign(iniItem.DefaultValue);
            Value = CreateInstanceOrAssign(iniItem.Value);
        }

        static private T CreateInstanceOrAssign()
        {
            if (typeof(T) == typeof(string))
                return (T)(object)"";

            if (typeof(T).IsValueType)
                return default!;

            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor != null)
                return (T)constructor.Invoke(null);

            throw new InvalidOperationException($"Type {typeof(T)} does not have a parameterless constructor.");
        }

        private T CreateInstanceOrAssign(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var constructor = typeof(T).GetConstructor(new[] { typeof(T) });
            if (constructor != null)
                return (T)constructor.Invoke(new object[] { value });

            return value;
        }
    }
}
