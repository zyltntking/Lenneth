using System.Collections.Generic;

namespace Lenneth.Core.Framework.DynamicData
{
    internal sealed class GroupChangeSet<TObject, TKey, TGroupKey> : ChangeSet<IGroup<TObject, TKey, TGroupKey>, TGroupKey>, IGroupChangeSet<TObject, TKey, TGroupKey>
    {

        public new static readonly IGroupChangeSet<TObject, TKey, TGroupKey> Empty = new GroupChangeSet<TObject, TKey, TGroupKey>();

        private GroupChangeSet()
        {
        }

        public GroupChangeSet(IEnumerable<Change<IGroup<TObject, TKey, TGroupKey>, TGroupKey>> items)
            : base(items)
        {
        }
    }
}
