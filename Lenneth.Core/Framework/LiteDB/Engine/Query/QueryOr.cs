﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.LiteDB
{
    internal class QueryOr : Query
    {
        private Query _left;
        private Query _right;

        public QueryOr(Query left, Query right)
            : base(null)
        {
            _left = left;
            _right = right;
        }

        internal override IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
        {
            var left = _left.Run(col, indexer);
            var right = _right.Run(col, indexer);

            // if any query (left/right) is FullScan, this query is full scan too
            UseIndex = _left.UseIndex && _right.UseIndex;
            UseFilter = _left.UseFilter || _right.UseFilter;

            return left.Union(right, new IndexNodeComparer());
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            throw new NotSupportedException();
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return _left.FilterDocument(doc) || _right.FilterDocument(doc);
        }

        public override string ToString()
        {
            return $"({_left} or {_right})";
        }
    }
}