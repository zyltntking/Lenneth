using System;
using System.Linq;

namespace Lenneth.Core.Framework.LiteDB
{
    public partial class LiteEngine
    {
        /// <summary>
        /// Implement delete command based on _id value. Returns true if deleted
        /// </summary>
        public bool Delete(string collection, BsonValue id)
        {
            return Delete(collection, Query.EQ("_id", id)) == 1;
        }

        /// <summary>
        /// Implements delete based on a query result
        /// </summary>
        public int Delete(string collection, Query query)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return Transaction<int>(collection, false, (col) =>
            {
                if (col == null) return 0;

                _log.Write(Logger.Command, "delete documents in '{0}'", collection);

                var nodes = query.Run(col, _indexer);

                _log.Write(Logger.Query, "{0} :: {1}", collection, query);

                var count = 0;

                foreach (var node in nodes)
                {
                    // checks if cache are full
                    _trans.CheckPoint();

                    // if use filter need deserialize document
                    if (query.UseFilter)
                    {
                        var buffer = _data.Read(node.DataBlock);
                        var doc = _bsonReader.Deserialize(buffer).AsDocument;

                        if (query.FilterDocument(doc) == false) continue;
                    }

                    _log.Write(Logger.Command, "delete document :: _id = {0}", node.Key.RawValue);

                    // get all indexes nodes from this data block
                    var allNodes = _indexer.GetNodeList(node, true).ToArray();

                    // lets remove all indexes that point to this in dataBlock
                    foreach (var linkNode in allNodes)
                    {
                        var index = col.Indexes[linkNode.Slot];

                        _indexer.Delete(index, linkNode.Position);
                    }

                    // remove object data
                    _data.Delete(col, node.DataBlock);

                    count++;
                }

                return count;
            });
        }
    }
}