using System;

namespace SimpleFileIO.Utility
{
    /// <summary>
    /// Provides a mechanism for converting between strings and specific object types.
    /// This structure allows defining custom parsing logic for type conversion.
    /// </summary>
    public struct StringTypeParser
    {
        /// <summary>
        /// Gets or sets the target type that this parser is designed to handle.
        /// This represents the type to which strings will be converted.
        /// </summary>
        public Type TargetType;

        /// <summary>
        /// A delegate that defines how a string should be converted into an object of <see cref="TargetType"/>.
        /// The function should take a string input and return an object of the expected type, or <c>null</c> if conversion fails.
        /// </summary>
        public Func<string, object?> StringToObject;

        /// <summary>
        /// A delegate that defines how an object of <see cref="TargetType"/> should be converted back into a string.
        /// The function should take an object and return its string representation, or <c>null</c> if conversion fails.
        /// </summary>
        public Func<object, string?> ObjectToString;
    }
}
