using System.Collections.Generic;
using System.Linq;

namespace Lenneth.Core.Framework.LiteDB
{
    internal class QueryIn : Query
    {
        private IEnumerable<BsonValue> _values;

        public QueryIn(string field, IEnumerable<BsonValue> values)
            : base(field)
        {
            _values = values ?? Enumerable.Empty<BsonValue>();
        }

        internal override IEnumerable<IndexNode> ExecuteIndex(IndexService indexer, CollectionIndex index)
        {
            foreach (var value in _values.Distinct())
            {
                foreach (var node in EQ(Field, value).ExecuteIndex(indexer, index))
                {
                    yield return node;
                }
            }
        }

        internal override bool FilterDocument(BsonDocument doc)
        {
            foreach(var val in Expression.Execute(doc, true))
            {
                foreach (var value in _values.Distinct())
                {
                    var diff = val.CompareTo(value);

                    if (diff == 0) return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}({1} in {2})",
                UseFilter ? "Filter" : UseIndex ? "Seek" : "",
                Expression?.ToString() ?? Field,
                 string.Join(",", _values.Select(a => a != null ? a.ToString() : "Null" ).ToArray()));
        }
    }
}