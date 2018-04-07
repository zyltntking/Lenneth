using System.Collections.Generic;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;

namespace Lenneth.Core.Framework.ObjectMapper.Core.Extensions
{
    internal static class DictionaryExtensions
    {
        public static Option<TValue> GetValue<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
        {
            var exists = value.TryGetValue(key, out var result);
            return new Option<TValue>(result, exists);
        }
    }
}
