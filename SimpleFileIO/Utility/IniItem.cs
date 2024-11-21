namespace SimpleFileIO.Utility
{
    public class IniItem<T> where T : notnull
    {
        public string Section { get; set; } = "";
        public string Key { get; set; } = "";
        public T DefaultValue { get; set; } = CreateInstanceOrAssign();
        public T Value { get; set; } = CreateInstanceOrAssign();
        public bool IsValidSectionKey { get => !string.IsNullOrWhiteSpace(Section) && !string.IsNullOrWhiteSpace(Key); }

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
            if (typeof(T).IsValueType)
            {
#pragma warning disable CS8600
                T result = default;
#pragma warning restore CS8600
                if (result is null)
                    throw new ArgumentNullException($"Type {typeof(T)} default value is null.");
                return result;
            }

            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor != null)
                return (T)constructor.Invoke(null);
            throw new InvalidOperationException($"Type {typeof(T)} does not have a parameterless constructor.");
        }

        private T CreateInstanceOrAssign(T value)
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(T) });
            if (constructor != null)
                return (T)constructor.Invoke(new object[] { value });
            else
                return value;
        }
    }
}
