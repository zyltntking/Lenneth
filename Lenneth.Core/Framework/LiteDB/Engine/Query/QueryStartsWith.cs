﻿using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.LiteDB
{
    internal class QueryStartsWith : Query
    {
        private BsonValue _value;

        public QueryStartsWith(string field, BsonValue value)
            : base(field)
        {
            _value = value;
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            // find first indexNode
            var node = indexer.Find(index, _value, true, Ascending);
            var str = _value.AsString;

            // navigate using next[0] do next node - if less or equals returns
            while (node != null)
            {
                var valueString = node.Key.AsString;

                // value will not be null because null occurs before string (bsontype sort order)
                if (valueString.StartsWith(str))
                {
                    if (!node.DataBlock.IsEmpty)
                    {
                        yield return node;
                    }
                }
                else
                {
                    break; // if no more starts with, stop scanning
                }

                node = indexer.GetNode(node.Next[0]);
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            return Expression.Execute(doc, false)
                .Where(x => x.IsString)
                .Any(x => x.AsString.StartsWith(_value));
        }

        public override string ToString()
        {
            return
                $"{(UseFilter ? "Filter" : UseIndex ? "Seek" : "")}({Expression?.ToString() ?? Field} startsWith {_value})";
        }
    }
}