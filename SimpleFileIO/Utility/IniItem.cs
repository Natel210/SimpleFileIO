using SimpleFileIO.Resources.ErrorMessages;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleFileIO.Utility
{
    /// <summary>
    /// Represents an INI configuration item with a section, key, and values.
    /// This class supports generic types with constraints ensuring they are not null.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the INI item.</typeparam>
    public class IniItem<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T> where T : notnull
    {
        /// <summary>
        /// The section name in the INI file.
        /// </summary>
        public string Section { get; set; } = "";

        /// <summary>
        /// The key name within the section in the INI file.
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// The default value associated with the key, used if the key is missing.
        /// </summary>
        public T DefaultValue { get; set; } = CreateInstanceOrAssign();

        /// <summary>
        /// The actual value stored in the INI file.
        /// </summary>
        public T Value { get; set; } = CreateInstanceOrAssign();

        /// <summary>
        /// Checks if the section and key are valid (i.e., non-empty strings).
        /// </summary>
        public bool IsValidSectionKey => !string.IsNullOrWhiteSpace(Section) && !string.IsNullOrWhiteSpace(Key);

        /// <summary>
        /// Default constructor that initializes an empty INI item.
        /// </summary>
        public IniItem()
        {
        }

        /// <summary>
        /// Copy constructor that creates a new instance from an existing <see cref="IniItem{T}"/>.
        /// </summary>
        /// <param name="iniItem">The existing INI item to copy values from.</param>
        /// <exception cref="ArgumentNullException">Thrown if section, key, or values are null.</exception>
        public IniItem(IniItem<T> iniItem)
        {
            Section = iniItem.Section ?? throw new ArgumentNullException($"{ErrorMessages.ini_item_invaild_section}, {nameof(iniItem.Section)}");
            Key = iniItem.Key ?? throw new ArgumentNullException($"{ErrorMessages.ini_item_invaild_key}, {nameof(iniItem.Key)}");
            DefaultValue = CreateInstanceOrAssign(iniItem.DefaultValue);
            Value = CreateInstanceOrAssign(iniItem.Value);
        }

        /// <summary>
        /// Creates an instance of type <typeparamref name="T"/> or assigns a default value if applicable.
        /// </summary>
        /// <returns>A new instance of <typeparamref name="T"/> or a default value.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <typeparamref name="T"/> lacks a parameterless constructor.</exception>
        static private T CreateInstanceOrAssign()
        {
            if (typeof(T) == typeof(string))
                return (T)(object)""; // Ensure strings default to an empty string.

            if (typeof(T).IsValueType)
                return default!; // Return the default value for value types.

            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor != null)
                return (T)constructor.Invoke(null); // Create an instance using the parameterless constructor.

            throw new InvalidOperationException($"{ErrorMessages.ini_item_invaild_constructior}, {typeof(T)}");
        }

        /// <summary>
        /// Creates a new instance of type <typeparamref name="T"/> from an existing value.
        /// </summary>
        /// <param name="value">The value to copy.</param>
        /// <returns>A new instance or the provided value if no copy constructor is available.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided value is null.</exception>
        private T CreateInstanceOrAssign(T value)
        {
            if (value == null)
                throw new ArgumentNullException(ErrorMessages.ini_item_invaild_value);

            var constructor = typeof(T).GetConstructor(new[] { typeof(T) });
            if (constructor != null)
                return (T)constructor.Invoke(new object[] { value }); // Create a copy if a copy constructor exists.

            return value; // Return the original value if no copy constructor is found.
        }
    }
}
