﻿using System;
using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB
{
    public partial class LiteEngine
    {
        /// <summary>
        /// Implement upsert command to documents in a collection. Calls update on all documents,
        /// then any documents not updated are then attempted to insert.
        /// This will have the side effect of throwing if duplicate items are attempted to be inserted. Returns true if document is inserted
        /// </summary>
        public bool Upsert(string collection, BsonDocument doc, BsonType autoId = BsonType.ObjectId)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            return Upsert(collection, new BsonDocument[] { doc }, autoId) == 1;
        }

        /// <summary>
        /// Implement upsert command to documents in a collection. Calls update on all documents,
        /// then any documents not updated are then attempted to insert.
        /// This will have the side effect of throwing if duplicate items are attempted to be inserted.
        /// </summary>
        public int Upsert(string collection, IEnumerable<BsonDocument> docs, BsonType autoId = BsonType.ObjectId)
        {
            if (collection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(collection));
            if (docs == null) throw new ArgumentNullException(nameof(docs));

            return Transaction<int>(collection, true, (col) =>
            {
                var count = 0;

                foreach (var doc in docs)
                {
                    // first try update document (if exists _id)
                    // if not found, insert
                    if (doc["_id"] == BsonValue.Null || UpdateDocument(col, doc) == false)
                    {
                        InsertDocument(col, doc, autoId);
                        count++;
                    }

                    _trans.CheckPoint();
                }

                // returns how many document was inserted
                return count;
            });
        }
    }
}