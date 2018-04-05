﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal class LeftJoin<TLeft, TLeftKey, TRight, TRightKey, TDestination>
    {
        private readonly IObservable<IChangeSet<TLeft, TLeftKey>> _left;
        private readonly IObservable<IChangeSet<TRight, TRightKey>> _right;

        private readonly Func<TRight, TLeftKey> _rightKeySelector;
        private readonly Func<TLeftKey, TLeft, Optional<TRight>, TDestination> _resultSelector;

        public LeftJoin(IObservable<IChangeSet<TLeft, TLeftKey>> left,
            IObservable<IChangeSet<TRight, TRightKey>> right,
            Func<TRight, TLeftKey> rightKeySelector,
            Func<TLeftKey, TLeft, Optional<TRight>, TDestination> resultSelector)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
            _rightKeySelector = rightKeySelector ?? throw new ArgumentNullException(nameof(rightKeySelector));
            _resultSelector = resultSelector ?? throw new ArgumentNullException(nameof(resultSelector));
        }
        
        public IObservable<IChangeSet<TDestination, TLeftKey>> Run()
        {
            return Observable.Create<IChangeSet<TDestination, TLeftKey>>(observer =>
            {
                var locker = new object();

                //create local backing stores
                var leftCache = _left.Synchronize(locker).AsObservableCache(false);
                var rightCache = _right.Synchronize(locker).ChangeKey(_rightKeySelector).AsObservableCache(false);

                //joined is the final cache
                var joinedCache = new LockFreeObservableCache<TDestination, TLeftKey>();

                var leftLoader = leftCache.Connect()
                    .Subscribe(changes =>
                    {
                        joinedCache.Edit(innerCache =>
                        {
                            changes.ForEach(change =>
                            {
                                switch (change.Reason)
                                {
                                    case ChangeReason.Add:
                                    case ChangeReason.Update:
                                        //Update with left (and right if it is presents)
                                        var left = change.Current;
                                        var right = rightCache.Lookup(change.Key);
                                        innerCache.AddOrUpdate(_resultSelector(change.Key, left, right), change.Key);
                                        break;
                                    case ChangeReason.Remove:
                                        //remove from result because a left value is expected
                                        innerCache.Remove(change.Key);
                                        break;
                                    case ChangeReason.Refresh:
                                        //propagate upstream
                                        innerCache.Refresh(change.Key);
                                        break;
                                }
                            });
                        });
                    });

                var rightLoader = rightCache.Connect()
                    .Subscribe(changes =>
                    {
                        joinedCache.Edit(innerCache =>
                        {
                            changes.ForEach(change =>
                            {
                                var right = change.Current;
                                var left = leftCache.Lookup(change.Key);

                                switch (change.Reason)
                                {
                                    case ChangeReason.Add:
                                    case ChangeReason.Update:
                                        {
                                            if (left.HasValue)
                                            {
                                                //Update with left and right value
                                                innerCache.AddOrUpdate(_resultSelector(change.Key, left.Value, right),
                                                    change.Key);
                                            }
                                            else
                                            {
                                                //remove if it is already in the cache
                                                innerCache.Remove(change.Key);
                                            }
                                        }
                                        break;
                                    case ChangeReason.Remove:
                                        {
                                            if (left.HasValue)
                                            {
                                                //Update with no right value
                                                innerCache.AddOrUpdate(
                                                    _resultSelector(change.Key, left.Value, Optional<TRight>.None),
                                                    change.Key);
                                            }
                                            else
                                            {
                                                //remove if it is already in the cache
                                                innerCache.Remove(change.Key);
                                            }
                                        }
                                        break;
                                    case ChangeReason.Refresh:
                                        //propagate upstream
                                        innerCache.Refresh(change.Key);
                                        break;
                                }
                            });
                        });
                    });


                return new CompositeDisposable(
                    joinedCache.Connect().NotEmpty().SubscribeSafe(observer),
                    leftCache,
                    rightCache,
                    leftLoader,
                    joinedCache,
                    rightLoader);
            });
        }
    }
}
