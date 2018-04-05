using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Lenneth.Core.Framework.DynamicData.Cache.Internal
{
    internal class DynamicFilter<TObject, TKey>
    {
        private readonly IObservable<IChangeSet<TObject, TKey>> _source;
        private readonly IObservable<Func<TObject, bool>> _predicateChanged;
        private readonly IObservable<Unit> _refilterObservable;

        public DynamicFilter(IObservable<IChangeSet<TObject, TKey>> source,
            IObservable<Func<TObject, bool>> predicateChanged,
            IObservable<Unit> refilterObservable = null)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _predicateChanged = predicateChanged ?? throw new ArgumentNullException(nameof(predicateChanged));
            _refilterObservable = refilterObservable;
        }

        public IObservable<IChangeSet<TObject, TKey>> Run()
        {
            return Observable.Create<IChangeSet<TObject, TKey>>(observer =>
            {
                var allData = new Cache<TObject, TKey>();
                var filteredData = new ChangeAwareCache<TObject, TKey>();
                Func<TObject, bool> predicate = t => false;

                var locker = new object();

                var refresher = LatestPredicateObservable()
                    .Synchronize(locker)
                    .Select(p =>
                    {
                        //set the local predicate
                        predicate = p;

                        //reapply filter using all data from the cache
                        return  filteredData.RefreshFilteredFrom(allData, predicate);
                    });

                var dataChanged = _source
                   // .Finally(observer.OnCompleted)
                    .Synchronize(locker)
                    .Select(changes =>
                    {
                        //maintain all data [required to re-apply filter]
                        allData.Clone(changes); 

                        //maintain filtered data 
                        filteredData.FilterChanges(changes, predicate);

                        //get latest changes
                        return filteredData.CaptureChanges();
                    });

                return refresher
                    .Merge(dataChanged)
                    .NotEmpty()
                    .SubscribeSafe(observer);
            });
        }

        private IObservable<Func<TObject, bool>> LatestPredicateObservable()
        {
            return Observable.Create<Func<TObject, bool>>(observable =>
            {
                Func<TObject, bool> latest = t => false;

                observable.OnNext(latest);

                var predicateChanged = _predicateChanged
                    .Subscribe(predicate =>
                    {
                        latest = predicate;
                        observable.OnNext(latest);
                    });

                var reapplier = _refilterObservable == null
                    ? Disposable.Empty
                    : _refilterObservable.Subscribe(_ => observable.OnNext(latest));

                return new CompositeDisposable(predicateChanged, reapplier);
            });
        }
    }
}