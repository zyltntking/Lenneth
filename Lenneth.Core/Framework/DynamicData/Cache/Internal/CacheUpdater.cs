using System;
using System.Collections.Generic;
using System.Linq;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal class CacheUpdater<TObject, TKey> : ISourceUpdater<TObject, TKey>
    {
        private readonly ChangeAwareCache<TObject, TKey> _cache;
        private readonly IKeySelector<TObject, TKey> _keySelector;

        public CacheUpdater(ChangeAwareCache<TObject, TKey> cache, IKeySelector<TObject, TKey> keySelector = null)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _keySelector = keySelector;
        }

        public IEnumerable<TObject> Items => _cache.Items;

        public IEnumerable<TKey> Keys => _cache.Keys;

        public IEnumerable<KeyValuePair<TKey, TObject>> KeyValues => _cache.KeyValues;

        public Optional<TObject> Lookup(TKey key)
        {
            var item = _cache.Lookup(key);
            return item.HasValue ? item.Value : Optional.None<TObject>();
        }

        public void Load(IEnumerable<TObject> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            Clear();
            AddOrUpdate(items);
        }

        public void AddOrUpdate(IEnumerable<TObject> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            items.ForEach(AddOrUpdate);
        }

        public void AddOrUpdate(TObject item)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            var key = _keySelector.GetKey(item);
            _cache.AddOrUpdate(item, key);
        }

        public void AddOrUpdate(TObject item, IEqualityComparer<TObject> comparer)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            var key = _keySelector.GetKey(item);
            var oldItem = _cache.Lookup(key);
            if (oldItem.HasValue)
            {
                if (comparer.Equals(oldItem.Value, item))
                    return;
                _cache.AddOrUpdate(item, key);
                return;
            }
            _cache.AddOrUpdate(item, key);
        }

        public TKey GetKey(TObject item)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            return _keySelector.GetKey(item);
        }


        public IEnumerable<KeyValuePair<TKey, TObject>> GetKeyValues(IEnumerable<TObject> items)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            return items.Select(t => new KeyValuePair<TKey, TObject>(_keySelector.GetKey(t), t));
        }

        public void AddOrUpdate(IEnumerable<KeyValuePair<TKey, TObject>> itemsPairs)
        {
            itemsPairs.ForEach(AddOrUpdate);
        }

        public void AddOrUpdate(KeyValuePair<TKey, TObject> item)
        {
            _cache.AddOrUpdate(item.Value, item.Key);
        }

        public void AddOrUpdate(TObject item, TKey key)
        {
            _cache.AddOrUpdate(item, key);
        }

        public void Refresh()
        {
            _cache.Refresh();
        }

        public void Refresh(IEnumerable<TObject> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            items.ForEach(Refresh);
        }

        public void Refresh(IEnumerable<TKey> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            keys.ForEach(Refresh);
        }

        public void Refresh(TObject item)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            var key = _keySelector.GetKey(item);
            _cache.Refresh(key);
        }

        [Obsolete(Constants.EvaluateIsDead)]
        public void Evaluate(IEnumerable<TKey> keys) => Refresh(keys);

        [Obsolete(Constants.EvaluateIsDead)]
        public void Evaluate(IEnumerable<TObject> items) => Refresh(items);

        [Obsolete(Constants.EvaluateIsDead)]
        public void Evaluate(TObject item) => Refresh(item);

        public void Refresh(TKey key)
        {
            _cache.Refresh(key);
        }

        [Obsolete(Constants.EvaluateIsDead)]
        public void Evaluate()
        {
            Refresh();
        }

        [Obsolete(Constants.EvaluateIsDead)]
        public void Evaluate(TKey key)
        {
            Refresh(key);
        }

        public void Remove(IEnumerable<TObject> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            items.ForEach(Remove);
        }

        public void Remove(IEnumerable<TKey> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            keys.ForEach(Remove);
        }

        public void RemoveKeys(IEnumerable<TKey> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            keys.ForEach(RemoveKey);
        }

        public void Remove(TObject item)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            var key = _keySelector.GetKey(item);
            _cache.Remove(key);
        }

        public Optional<TObject> Lookup(TObject item)
        {
            if (_keySelector == null)
                throw new KeySelectorException("A key selector must be specified");

            TKey key = _keySelector.GetKey(item);
            return Lookup(key);
        }

        public void Remove(TKey key)
        {
            _cache.Remove(key);
        }

        public void RemoveKey(TKey key)
        {
            Remove(key);
        }

        public void Remove(IEnumerable<KeyValuePair<TKey, TObject>> items)
        {
            items.ForEach(Remove);
        }

        public void Remove(KeyValuePair<TKey, TObject> item)
        {
            Remove(item.Key);
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public int Count => _cache.Count;


        public void Update(IChangeSet<TObject, TKey> changes)
        {
            _cache.Clone(changes);
        }

        public IChangeSet<TObject, TKey> AsChangeSet()
        {
            return _cache.CaptureChanges();
        }
    }
}
