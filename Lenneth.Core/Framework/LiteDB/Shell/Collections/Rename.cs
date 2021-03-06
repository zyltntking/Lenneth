﻿using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    [Help(
        Category = "Collection",
        Name = "rename",
        Syntax = "db.<collection>.rename <new_name>",
        Description = "Rename a collection. New name can't exists",
        Examples = new string[] {
            "db.customers.rename new_cust"
        }
    )]
    internal class CollectionRename : BaseCollection, ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return IsCollectionCommand(s, "rename");
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            var col = ReadCollection(engine, s);
            var newName = s.Scan(@"[\w-]+").ThrowIfEmpty("Invalid new collection name", s);

            s.ThrowIfNotFinish();

            yield return engine.RenameCollection(col, newName);
        }
    }
}