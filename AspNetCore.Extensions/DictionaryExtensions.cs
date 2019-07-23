namespace AspNetCore.Extensions
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using JetBrains.Annotations;

    [DebuggerStepThrough]
    internal static class DictionaryExtensions
    {
        public static TValue GetOrAddNew<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> source,
            [NotNull] TKey key)
            where TValue : new()
        {
            if (!source.TryGetValue(key, out var value))
            {
                value = new TValue();
                source.Add(key, value);
            }

            return value;
        }

        public static TValue Find<TKey, TValue>(
            [NotNull] this IReadOnlyDictionary<TKey, TValue> source,
            [NotNull] TKey key)
            => !source.TryGetValue(key, out var value) ? default : value;
    }
}
