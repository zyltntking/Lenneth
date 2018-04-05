﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.List.Internal
{
    internal sealed class GroupOnPropertyWithImmutableState<TObject, TGroup>
        where TObject : INotifyPropertyChanged
    {
        private readonly IObservable<IChangeSet<TObject>> _source;
        private readonly Expression<Func<TObject, TGroup>> _propertySelector;
        private readonly TimeSpan? _throttle;
        private readonly IScheduler _scheduler;
        private readonly Func<TObject, TGroup> _groupSelector;

        public GroupOnPropertyWithImmutableState(IObservable<IChangeSet<TObject>> source, Expression<Func<TObject, TGroup>> groupSelectorKey, TimeSpan? throttle = null, IScheduler scheduler = null)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _groupSelector = groupSelectorKey?.Compile() ?? throw new ArgumentNullException(nameof(groupSelectorKey));
            _propertySelector = groupSelectorKey;
            _throttle = throttle;
            _scheduler = scheduler;
        }

        public IObservable<IChangeSet<IGrouping<TObject, TGroup>>> Run()
        {
            return _source.Publish(shared =>
            {
                // Monitor explicit property changes
                var regrouper = ObservableListEx.WhenValueChanged(shared, _propertySelector, (bool) false).ToUnit();

                //add a throttle if specified
                if (_throttle != null)
                    regrouper = regrouper.Throttle(_throttle.Value, _scheduler ?? Scheduler.Default);

                // Use property changes as a trigger to re-evaluate Grouping
                return shared.GroupWithImmutableState(_groupSelector, regrouper);
            });
        }
    }
}