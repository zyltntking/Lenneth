using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Lenneth.Core.Framework.DynamicData.Annotations;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.List.Internal
{
    internal class QueryWhenChanged<T>
    {
        private readonly IObservable<IChangeSet<T>> _source;

        public QueryWhenChanged([NotNull] IObservable<IChangeSet<T>> source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IObservable<IReadOnlyCollection<T>> Run()
        {
            return _source.Scan(new List<T>(), (list, changes) =>
                {
                    ListEx.Clone<T>(list, changes);
                    return list;
                }
            ).Select(list => new ReadOnlyCollectionLight<T>(list));
        }
    }
}
