﻿using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    [Help(
        Category = "FileStorage",
        Name = "delete",
        Syntax = "fs.delete <fileId>",
        Description = "Delete a file using fileId key. Return true if file has been deleted.",
        Examples = new string[] {
            "fs.delete my_photo_001"
        }
    )]
    internal class FileDelete : BaseStorage, ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return IsFileCommand(s, "delete");
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            var fs = new LiteStorage(engine);
            var id = ReadId(s);

            s.ThrowIfNotFinish();

            yield return fs.Delete(id);
        }
    }
}