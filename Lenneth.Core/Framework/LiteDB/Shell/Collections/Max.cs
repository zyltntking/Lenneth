﻿using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    [Help(
        Category = "Collection",
        Name = "max",
        Syntax = "db.<collection>.max [<field>]",
        Description = "Returns max/last value from collection using index field. Use default _id index if not defined",
        Examples = new string[] {
            "db.orders.max age"
        }
    )]
    internal class CollectionMax : BaseCollection, ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return IsCollectionCommand(s, "max");
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            var col = ReadCollection(engine, s);
            var index = s.Scan(FieldPattern).Trim();

            if (!s.HasTerminated) throw LiteException.SyntaxError(s, "Invalid field/index name");

            yield return engine.Max(col, index.Length == 0 ? "_id" : index);
        }
    }
}