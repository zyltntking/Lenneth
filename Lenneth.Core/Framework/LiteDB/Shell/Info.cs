using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    internal class Info : ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return s.Scan(@"db.info$").Length > 0;
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            yield return engine.Info();
        }
    }
}