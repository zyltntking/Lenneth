using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Lenneth.Core.Framework.DynamicData.Annotations;

namespace Lenneth.Core.Framework.DynamicData.List.Internal
{
    internal sealed class LimitSizeTo<T>
    {
        private readonly ISourceList<T> _sourceList;
        private readonly IScheduler _scheduler;
        private readonly object _locker;
        private readonly int _sizeLimit;

        public LimitSizeTo([NotNull] ISourceList<T> sourceList, int sizeLimit, [NotNull] IScheduler scheduler, object locker)
        {
            _sourceList = sourceList ?? throw new ArgumentNullException(nameof(sourceList));
            _sizeLimit = sizeLimit;
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _locker = locker;
        }

        public IObservable<IEnumerable<T>> Run()
        {
            var emptyResult = new List<T>();
            long orderItemWasAdded = -1;

            return _sourceList.Connect()
                              .ObserveOn(_scheduler)
                              .Synchronize(_locker)
                              .Transform(t => new ExpirableItem<T>(t, _scheduler.Now.DateTime, Interlocked.Increment(ref orderItemWasAdded)))
                              .ToCollection()
                              .Select(list =>
                              {
                                  var numbertoExpire = list.Count - _sizeLimit;
                                  if (numbertoExpire < 0)
                                      return emptyResult;

                                  var dueForExpiry = list.OrderBy(exp => exp.ExpireAt).ThenBy(exp => exp.Index)
                                                         .Take(numbertoExpire)
                                                         .Select(item => item.Item)
                                                         .ToList();
                                  return dueForExpiry;
                              }).Where(items => items.Count != 0);
        }
    }
}
