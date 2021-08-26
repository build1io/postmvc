using System.Collections.Generic;

namespace Build1.PostMVC.Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static V GetOrDefault<K, V>(this IReadOnlyDictionary<K, V> dictionary, K key)
        {
            return dictionary.GetOrDefault(key, default);
        }

        public static V GetOrDefault<K, V>(this IReadOnlyDictionary<K, V> dictionary, K key, V @default)
        {
            return dictionary.TryGetValue(key, out var value) ? value : @default;
        }
    }
}
