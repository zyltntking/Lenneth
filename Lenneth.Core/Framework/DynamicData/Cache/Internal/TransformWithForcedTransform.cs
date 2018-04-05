using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal sealed class TransformWithForcedTransform<TDestination, TSource, TKey>
    {
        private readonly IObservable<IChangeSet<TSource, TKey>> _source;
        private readonly Func<TSource, Optional<TSource>, TKey, TDestination> _transformFactory;
        private readonly IObservable<Func<TSource, TKey, bool>> _forceTransform;
        private readonly Action<Error<TSource, TKey>> _exceptionCallback;

        public TransformWithForcedTransform(IObservable<IChangeSet<TSource, TKey>> source,
            Func<TSource, Optional<TSource>, TKey, TDestination> transformFactory,
            IObservable<Func<TSource, TKey, bool>> forceTransform,
            Action<Error<TSource, TKey>> exceptionCallback = null)
        {
            _source = source;
            _exceptionCallback = exceptionCallback;
            _transformFactory = transformFactory;
            _forceTransform = forceTransform;
        }

        public IObservable<IChangeSet<TDestination, TKey>> Run()
        {
            return Observable.Create<IChangeSet<TDestination, TKey>>(observer =>
            {
                var locker = new object();
                var shared = _source.Synchronize(locker).Publish();

                //capture all items so we can apply a forced transform
                var cache = new Cache<TSource, TKey>();
                var cacheLoader = shared.Subscribe(changes => cache.Clone(changes));

                //create change set of items where force refresh is applied
                var refresher = _forceTransform.Synchronize(locker)
                    .Select(selector => CaptureChanges(cache, selector))
                    .Select(changes => new ChangeSet<TSource, TKey>(changes))
                    .NotEmpty();

                var sourceAndRefreshes = shared.Merge(refresher);

                //do raw transform
                var transform = new Transform<TDestination, TSource, TKey>(sourceAndRefreshes, _transformFactory, _exceptionCallback, true).Run();

                return new CompositeDisposable(cacheLoader,transform.SubscribeSafe(observer), shared.Connect());
            });

        }

        private IEnumerable<Change<TSource, TKey>> CaptureChanges(Cache<TSource, TKey>  cache, Func<TSource, TKey, bool> shouldTransform)
        {
            foreach (var kvp in cache.KeyValues)
            {
                if (shouldTransform(kvp.Value, kvp.Key))
                    yield return new Change<TSource, TKey>(ChangeReason.Refresh, kvp.Key,kvp.Value);
            }
        }
    }
}