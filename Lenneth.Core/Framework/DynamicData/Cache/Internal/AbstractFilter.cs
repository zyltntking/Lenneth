using System;
using System.Collections.Generic;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal abstract class AbstractFilter<TObject, TKey> : IFilter<TObject, TKey>
    {
        private readonly ChangeAwareCache<TObject, TKey> _cache;

        protected AbstractFilter(ChangeAwareCache<TObject, TKey> cache, Func<TObject, bool> filter)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

            if (filter == null)
            {
                Filter = t => true;
            }
            else
            {
                Filter = filter;
            }
        }

        public Func<TObject, bool> Filter { get; }

        public IChangeSet<TObject, TKey> Refresh(IEnumerable<KeyValuePair<TKey, TObject>> items)
        {
            //this is an internal method only so we can be sure there are no duplicate keys in the result
            //(therefore safe to parallelise)
            Optional<Change<TObject, TKey>> Factory(KeyValuePair<TKey, TObject> kv)
            {
                var exisiting = _cache.Lookup(kv.Key);
                var matches = Filter(kv.Value);

                if (matches)
                {
                    if (!exisiting.HasValue)
                        return new Change<TObject, TKey>(ChangeReason.Add, kv.Key, kv.Value);
                }
                else
                {
                    if (exisiting.HasValue)
                        return new Change<TObject, TKey>(ChangeReason.Remove, kv.Key, kv.Value, exisiting);
                }

                return Optional.None<Change<TObject, TKey>>();
            }

            var result = Refresh(items, Factory);
            _cache.Clone(new ChangeSet<TObject, TKey>(result));


            return _cache.CaptureChanges();
        }

        protected abstract IEnumerable<Change<TObject, TKey>> Refresh(IEnumerable<KeyValuePair<TKey, TObject>> items, Func<KeyValuePair<TKey, TObject>, Optional<Change<TObject, TKey>>> factory);

        public IChangeSet<TObject, TKey> Update(IChangeSet<TObject, TKey> updates)
        {
            var withfilter = GetChangesWithFilter(updates);
            return ProcessResult(withfilter);
        }

        protected abstract IEnumerable<UpdateWithFilter> GetChangesWithFilter(IChangeSet<TObject, TKey> updates);

        private IChangeSet<TObject, TKey> ProcessResult(IEnumerable<UpdateWithFilter> source)
        {
            //Have to process one item at a time as an item can be included multiple
            //times in any batch

            foreach (var item in source)
            {
                var matches = item.IsMatch;
                var key = item.Change.Key;
                var u = item.Change;

                switch (item.Change.Reason)
                {
                    case ChangeReason.Add:
                        {
                            if (matches)
                                _cache.AddOrUpdate(u.Current, u.Key);
                        }
                        break;
                    case ChangeReason.Update:
                        {
                            if (matches)
                            {
                                _cache.AddOrUpdate(u.Current, u.Key);
                            }
                            else
                            {
                                _cache.Remove(u.Key);
                            }
                        }
                        break;
                    case ChangeReason.Remove:
                        _cache.Remove(u.Key);
                        break;
                    case ChangeReason.Refresh:
                        {
                            var exisiting = _cache.Lookup(key);
                            if (matches)
                            {
                                if (!exisiting.HasValue)
                                {
                                    _cache.AddOrUpdate(u.Current, u.Key);
                                }
                                else
                                {
                                    _cache.Refresh();
                                }
                            }
                            else
                            {
                                if (exisiting.HasValue)
                                    _cache.Remove(u.Key);
                            }
                        }
                        break;
                }
            }
            return _cache.CaptureChanges();
        }


        protected struct UpdateWithFilter
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public UpdateWithFilter(bool isMatch, Change<TObject, TKey> change)
            {
                IsMatch = isMatch;
                Change = change;
            }

            public Change<TObject, TKey> Change { get; }
            public bool IsMatch { get; }
        }
    }
}
