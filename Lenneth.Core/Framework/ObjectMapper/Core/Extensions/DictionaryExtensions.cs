using System.Collections.Generic;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;

namespace Lenneth.Core.Framework.ObjectMapper.Core.Extensions
{
    internal static class DictionaryExtensions
    {
        public static Option<TValue> GetValue<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
        {
            TValue result;
            bool exists = value.TryGetValue(key, out result);
            return new Option<TValue>(result, exists);
        }
    }
}
