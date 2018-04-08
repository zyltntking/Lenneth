using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class DictionaryExtensions
    {
        public static IDictionary<TV, TK> Invert<TK, TV>(
            this IDictionary<TK, TV> aDictionary)
        {
            return aDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }
    }
}