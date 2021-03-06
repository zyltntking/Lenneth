﻿using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.LiteDB
{
    internal class QueryBetween : Query
    {
        private BsonValue _start;
        private BsonValue _end;

        private bool _startEquals;
        private bool _endEquals;

        public QueryBetween(string field, BsonValue start, BsonValue end, bool startEquals, bool endEquals)
            : base(field)
        {
            _start = start;
            _startEquals = startEquals;
            _end = end;
            _endEquals = endEquals;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            // define order
            var order = _start.CompareTo(_end) <= 0 ? Ascending : Descending;

            // find first indexNode
            var node = indexer.Find(index, _start, true, order);

            // returns (or not) equals start value
            while (node != null)
            {
                var diff = node.Key.CompareTo(_start);

                // if current value are not equals start, go out this loop
                if (diff != 0) break;

                if (_startEquals)
                {
                    yield return node;
                }

                node = indexer.GetNode(node.NextPrev(0, order));
            }

            // navigate using next[0] do next node - if less or equals returns
            while (node != null)
            {
                var diff = node.Key.CompareTo(_end);

                if (_endEquals && diff == 0)
                {
                    yield return node;
                }
                else if (diff == -order)
                {
                    yield return node;
                }
                else
                {
                    break;
                }

                node = indexer.GetNode(node.NextPrev(0, order));
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return Expression.Execute(doc, false)
                .Any(x =>
                {
                    return
                        (_startEquals ? x.CompareTo(_start) >= 0 : x.CompareTo(_start) > 0) &&
                        (_endEquals ? x.CompareTo(_end) <= 0 : x.CompareTo(_end) < 0);
                });
        }

        public override string ToString()
        {
            return
                $"{(UseFilter ? "Filter" : UseIndex ? "IndexSeek" : "")}({Expression?.ToString() ?? Field} between {(_startEquals ? "[" : "(")}{_start} and {_end}{(_endEquals ? "]" : ")")})";
        }
    }
}