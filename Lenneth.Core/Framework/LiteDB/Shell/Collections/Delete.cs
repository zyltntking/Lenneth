using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB.Shell
{
    [Help(
        Category = "Collection",
        Name = "delete",
        Syntax = "db.<collection>.delete [filter]",
        Description = "Delete documents according filter clause (required). Retruns deleted document count.",
        Examples = new string[] {
            "db.orders.delete _id = 2",
            "db.orders.delete customer = \"John Doe\"",
            "db.orders.delete customer startsWith \"John\" and YEAR($.orderDate) >= 2015"
        }
    )]
    internal class CollectionDelete : BaseCollection, ICommand
    {
        public bool IsCommand(StringScanner s)
        {
            return IsCollectionCommand(s, "delete");
        }

        public IEnumerable<BsonValue> Execute(StringScanner s, LiteEngine engine)
        {
            var col = ReadCollection(engine, s);
            var query = ReadQuery(s, true);

            s.ThrowIfNotFinish();

            yield return engine.Delete(col, query);
        }
    }
}