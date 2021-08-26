using System.Collections.Generic;

namespace Build1.PostMVC.Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static V GetOrDefault<K, V>(this IReadOnlyDictionary<K, V> instance, K key)
        {
            return instance.GetOrValue(key, default);
        }

        public static V GetOrValue<K, V>(this IReadOnlyDictionary<K, V> instance, K key, V defaultValue)
        {
            if (instance.TryGetValue(key, out var existingValue))
            {
                return existingValue;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
