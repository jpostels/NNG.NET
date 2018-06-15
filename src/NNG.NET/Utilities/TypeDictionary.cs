namespace NNGNET.Utilities
{
    /// <summary>
    ///     Provides a dictionary like behaviour with a type as a lookup key.
    /// </summary>
    /// <remarks>
    ///     Uses .NETs generic type system to increase performance over conventional <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> with <see cref="System.Type"/> as key.
    /// </remarks>
    /// <typeparam name="TValue">The type of the stored values.</typeparam>
    internal static class TypeDictionary<TValue>
    {
        /// <summary>
        ///     Gets the value associated with the key type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The value</returns>
        public static TValue Get<T>()
        {
            return ValueStore<T, TValue>.Value;
        }

        /// <summary>
        ///     Sets the value associated with the key type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        public static void Set<T>(TValue value)
        {
            ValueStore<T, TValue>.Value = value;
        }

        /// <summary>
        ///     Value holder using <typeparamref name="TType"/> as key.
        /// </summary>
        /// <typeparam name="TType">The key specified as type.</typeparam>
        /// <typeparam name="TStoredValue">The type of the stored value.</typeparam>
        private static class ValueStore<TType, TStoredValue>
        {
            public static TStoredValue Value;
        }
    }
}
