using System.Collections.Generic;

namespace Lenneth.Core.Framework.DynamicData
{
    internal class DistinctChangeSet<T> : ChangeSet<T, T>, IDistinctChangeSet<T>
    {
        public DistinctChangeSet(IEnumerable<Change<T, T>> items)
            : base(items)
        {
        }
    }
}
