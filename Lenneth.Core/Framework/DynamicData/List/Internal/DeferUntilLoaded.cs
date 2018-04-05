using System;
using System.Reactive.Linq;
using Lenneth.Core.Framework.DynamicData.Annotations;
using Lenneth.Core.Framework.DynamicData.Kernel;

namespace Lenneth.Core.Framework.DynamicData.List.Internal
{
    internal class DeferUntilLoaded<T>
    {
        private readonly IObservable<IChangeSet<T>> _source;

        public DeferUntilLoaded([NotNull] IObservable<IChangeSet<T>> source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public IObservable<IChangeSet<T>> Run()
        {
            return _source.MonitorStatus()
                          .Where(status => status == ConnectionStatus.Loaded)
                          .Take(1)
                          .Select(_ => new ChangeSet<T>())
                          .Concat(_source)
                          .NotEmpty();
        }
    }
}
