using System.Collections.Generic;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal sealed class AnonymousQuery<TObject, TKey> : IQuery<TObject, TKey>
    {
        private readonly Cache<TObject, TKey> _cache;

        public AnonymousQuery(Cache<TObject, TKey> cache)
        {
            _cache = cache.Clone();
        }

        public int Count => _cache.Count;

        public IEnumerable<TObject> Items => _cache.Items;

        public IEnumerable<KeyValuePair<TKey, TObject>> KeyValues => _cache.KeyValues;

        public IEnumerable<TKey> Keys => _cache.Keys;

        public Optional<TObject> Lookup(TKey key)
        {
            return _cache.Lookup(key);
        }
    }
}
