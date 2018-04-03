﻿using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// All is an Index Scan operation
    /// </summary>
    internal class QueryAll : Query
    {
        private int _order;

        public QueryAll(string field, int order)
            : base(field)
        {
            _order = order;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            return indexer.FindAll(index, _order);
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return true;
        }

        public override string ToString()
        {
            return $"{(UseFilter ? "Filter" : UseIndex ? "Scan" : "")}({Expression?.ToString() ?? Field})";
        }
    }
}