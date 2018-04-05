using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Lenneth.Core.Framework.DynamicData.Cache.Internal;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData
{
    internal sealed class ObservableCache<TObject, TKey> : IObservableCache<TObject, TKey>, ICollectionSubject
    {
        private readonly ISubject<IChangeSet<TObject, TKey>> _changes = new Subject<IChangeSet<TObject, TKey>>();
        private readonly Lazy<ISubject<int>> _countChanged = new Lazy<ISubject<int>>(() => new Subject<int>());
        private readonly ReaderWriter<TObject, TKey> _readerWriter;
        private readonly IDisposable _cleanUp;
        private readonly object _locker = new object();
        private readonly object _writeLock = new object();

        public ObservableCache(IObservable<IChangeSet<TObject, TKey>> source)
        {
            _readerWriter = new ReaderWriter<TObject, TKey>();

            var loader = source
                .Synchronize(_locker)
                .Select(_readerWriter.Write)
                .Finally(_changes.OnCompleted)
                .Subscribe(InvokeNext, ex => _changes.OnError(ex));

            _cleanUp = Disposable.Create(() =>
            {
                loader.Dispose();
                _changes.OnCompleted();
                if (_countChanged.IsValueCreated)
                    _countChanged.Value.OnCompleted();
            });
        }

        public ObservableCache(Func<TObject, TKey> keySelector = null)
        {
            _readerWriter = new ReaderWriter<TObject, TKey>(keySelector);

            _cleanUp = Disposable.Create(() =>
            {
                _changes.OnCompleted();
                if (_countChanged.IsValueCreated)
                    _countChanged.Value.OnCompleted();
            });
        }

        internal void UpdateFromIntermediate(Action<ICacheUpdater<TObject, TKey>> updateAction)
        {
            if (updateAction == null) throw new ArgumentNullException(nameof(updateAction));
            lock (_writeLock)
            {
                InvokeNext(_readerWriter.Write(updateAction));
            }
        }

        internal void UpdateFromSource(Action<ISourceUpdater<TObject, TKey>> updateAction)
        {
            if (updateAction == null) throw new ArgumentNullException(nameof(updateAction));
            lock (_writeLock)
            {
                InvokeNext(_readerWriter.Write(updateAction));
            }
        }

        private void InvokeNext(IChangeSet<TObject, TKey> changes)
        {
            lock (_locker)
            {
                if (changes.Count == 0) return;
                _changes.OnNext(changes);

                if (_countChanged.IsValueCreated)
                    _countChanged.Value.OnNext(_readerWriter.Count);
            }
        }

        public IObservable<int> CountChanged => _countChanged.Value.StartWith(_readerWriter.Count).DistinctUntilChanged();

        public IObservable<Change<TObject, TKey>> Watch(TKey key)
        {
            return Observable.Create<Change<TObject, TKey>>
            (
                observer =>
                {
                    lock (_locker)
                    {
                        var initial = _readerWriter.Lookup(key);
                        if (initial.HasValue)
                            observer.OnNext(new Change<TObject, TKey>(ChangeReason.Add, key, initial.Value));

                        return _changes.Finally(observer.OnCompleted).Subscribe(changes =>
                        {
                            foreach (var match in changes.Where(update => update.Key.Equals(key)))
                            {
                                observer.OnNext(match);
                            }
                        });
                    }
                });
        }

        public IObservable<IChangeSet<TObject, TKey>> Connect(Func<TObject, bool> predicate = null)
        {
            return Observable.Defer(() =>
            {
                lock (_locker)
                {
                    var initial = GetInitialUpdates(predicate);
                    var changes = Observable.Return(initial).Concat(_changes);

                    return (predicate == null ? changes : changes.Filter(predicate))
                        .NotEmpty();
                }
            });
        }

        internal IChangeSet<TObject, TKey> GetInitialUpdates(Func<TObject, bool> filter = null) => _readerWriter.GetInitialUpdates(filter);

        public Optional<TObject> Lookup(TKey key) => _readerWriter.Lookup(key);

        public IEnumerable<TKey> Keys => _readerWriter.Keys;

        public IEnumerable<KeyValuePair<TKey, TObject>> KeyValues => _readerWriter.KeyValues;

        public IEnumerable<TObject> Items => _readerWriter.Items;

        public int Count => _readerWriter.Count;

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        void ICollectionSubject.OnCompleted()
        {
            lock (_locker)
                _changes.OnCompleted();
        }
        
        void ICollectionSubject.OnError(Exception exception)
        {
            lock (_locker)
                _changes.OnError(exception);
        }
    }
}
