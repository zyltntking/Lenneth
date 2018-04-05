﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Concurrency;

namespace Lenneth.Core.Framework.DynamicData.List.Internal
{
    [Obsolete("Use AutoRefresh(), followed by Filter() instead")]
    internal class FilterOnProperty<TObject, TProperty> where TObject : INotifyPropertyChanged
    {
        private readonly Func<TObject, bool> _predicate;
        private readonly TimeSpan? _throttle;
        private readonly IScheduler _scheduler;
        private readonly Expression<Func<TObject, TProperty>> _propertySelector;
        private readonly IObservable<IChangeSet<TObject>> _source;

        public FilterOnProperty(IObservable<IChangeSet<TObject>> source, Expression<Func<TObject, TProperty>> propertySelector, Func<TObject, bool> predicate,  TimeSpan? throttle = null, IScheduler scheduler = null)
        {
            _source = source;
            _propertySelector = propertySelector;
            _predicate = predicate;
            _throttle = throttle;
            _scheduler = scheduler;
        }

        public IObservable<IChangeSet<TObject>> Run()
        {
            return _source
                .AutoRefresh(_propertySelector, propertyChangeThrottle: _throttle, scheduler: _scheduler)
                .Filter(_predicate);
        }
    }
}
