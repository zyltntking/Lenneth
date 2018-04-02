using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    internal interface ICommand
    {
        bool IsCommand(StringScanner s);

        IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine);
    }
}