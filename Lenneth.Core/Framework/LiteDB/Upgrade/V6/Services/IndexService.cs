using System.Collections.Generic;
using Lenneth.Core.Framework.LiteDB;
using CollectionIndex = Lenneth.Core.Framework.LiteDB_V6.CollectionIndex;
using IndexNode = Lenneth.Core.Framework.LiteDB_V6.IndexNode;
using IndexPage = Lenneth.Core.Framework.LiteDB_V6.IndexPage;

namespace Lenneth.Core.Framework.LiteDB_V6
{
    /// <summary>
    /// Implement a Index service - Add/Remove index nodes on SkipList
    /// </summary>
    internal class IndexService
    {
        private PageService _pager;

        public IndexService(PageService pager)
        {
            _pager = pager;
        }

        /// <summary>
        /// Get a node inside a page using PageAddress - Returns null if address IsEmpty
        /// </summary>
        public IndexNode GetNode(PageAddress address)
        {
            if (address.IsEmpty) return null;
            var page = _pager.GetPage<IndexPage>(address.PageID);
            return page.Nodes[address.Index];
        }

        public IEnumerable<IndexNode> FindAll(CollectionIndex index)
        {
            var cur = this.GetNode(index.HeadNode);

            while (!cur.Next[0].IsEmpty)
            {
                cur = this.GetNode(cur.Next[0]);

                // stop if node is head/tail
                if (cur.IsHeadTail(index)) yield break;

                yield return cur;
            }
        }
    }
}