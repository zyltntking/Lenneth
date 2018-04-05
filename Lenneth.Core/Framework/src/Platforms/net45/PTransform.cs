#if P_LINQ

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData.Kernel;

namespace DynamicData.PLinq
{
    internal sealed class PTransform<TDestination, TSource, TKey>
    {
        private readonly IObservable<IChangeSet<TSource, TKey>> _source;
        private readonly Func<TSource, Optional<TSource>, TKey, TDestination> _transformFactory;
        private readonly ParallelisationOptions _parallelisationOptions;
        private readonly Action<Error<TSource, TKey>> _exceptionCallback;

        public PTransform(IObservable<IChangeSet<TSource, TKey>> source,
            Func<TSource, Optional<TSource>, TKey, TDestination> transformFactory,
            ParallelisationOptions parallelisationOptions,
            Action<Error<TSource, TKey>> exceptionCallback = null)
        {
            _source = source;
            _exceptionCallback = exceptionCallback;
            _transformFactory = transformFactory;
            _parallelisationOptions = parallelisationOptions;
        }

        public IObservable<IChangeSet<TDestination, TKey>> Run()
        {
            return Observable.Create<IChangeSet<TDestination, TKey>>(observer =>
            {
                var cache = new ChangeAwareCache<TDestination, TKey>();
                var transformer = _source.Select(changes => DoTransform(cache, changes));
                return transformer.NotEmpty().SubscribeSafe(observer);
            });
        }

        private IChangeSet<TDestination, TKey> DoTransform(ChangeAwareCache<TDestination, TKey> cache, IChangeSet<TSource, TKey> changes)
        {
         //   var transformed = changes.Select(ToDestination);


            var transformed = changes.ShouldParallelise(_parallelisationOptions)
                ? changes.Parallelise(_parallelisationOptions).Select(ToDestination).ToArray()
                : changes.Select(ToDestination).ToArray();

            return ProcessUpdates(cache, transformed);
        }

        private TransformResult ToDestination(Change<TSource, TKey> change)
        {
            try
            {
                if (change.Reason == ChangeReason.Add || change.Reason == ChangeReason.Update)
                {
                    var destination = _transformFactory(change.Current, change.Previous, change.Key);
                    return new TransformResult(change, destination);
                }
                return new TransformResult(change);
            }
            catch (Exception ex)
            {
                //only handle errors if a handler has been specified
                if (_exceptionCallback != null)
                    return new TransformResult(change, ex);
                throw;
            }
        }

        private IChangeSet<TDestination, TKey> ProcessUpdates(ChangeAwareCache<TDestination, TKey> cache, IEnumerable<TransformResult> transformedItems)
        {
            foreach (var result in transformedItems)
            {
                if (result.Success)
                {
                    switch (result.Change.Reason)
                    {
                        case ChangeReason.Add:
                        case ChangeReason.Update:
                            cache.AddOrUpdate(result.Destination.Value, result.Key);
                            break;

                        case ChangeReason.Remove:
                            cache.Remove(result.Key);
                            break;

                        case ChangeReason.Refresh:
                            cache.Refresh(result.Key);
                            break;
                    }
                }
                else
                {
                    _exceptionCallback(new Error<TSource, TKey>(result.Error, result.Change.Current, result.Change.Key));
                }
            }

            return cache.CaptureChanges();
        }

        private struct TransformResult
        {
            public Change<TSource, TKey> Change { get; }
            public Exception Error { get; }
            public bool Success { get; }
            public Optional<TDestination> Destination { get; }
            public TKey Key { get; }

            public TransformResult(Change<TSource, TKey> change, TDestination destination)
                : this()
            {
                Change = change;
                Destination = destination;
                Success = true;
                Key = change.Key;
            }


            public TransformResult(Change<TSource, TKey> change)
                : this()
            {
                Change = change;
                Destination = Optional<TDestination>.None;
                Success = true;
                Key = change.Key;
            }

            public TransformResult(Change<TSource, TKey> change, Exception error)
                : this()
            {
                Change = change;
                Error = error;
                Success = false;
                Key = change.Key;
            }
        }
    }
}

#endif