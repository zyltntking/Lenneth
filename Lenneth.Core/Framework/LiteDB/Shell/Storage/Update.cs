﻿using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    [Help(
        Category = "FileStorage",
        Name = "update",
        Syntax = "fs.update <fileId> <jsonDoc>",
        Description = "Update metada from a file inside file storage.",
        Examples = new string[] {
            "fs.update my_photo_001 { author: \"John Doe\" }"
        }
    )]
    internal class FileUpdate : BaseStorage, ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return IsFileCommand(s, "update");
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            var fs = new LiteStorage(engine);
            var id = ReadId(s);
            var metadata = JsonSerializer.Deserialize(s.ToString()).AsDocument;

            s.ThrowIfNotFinish();

            fs.SetMetadata(id, metadata);

            yield break;
        }
    }
}