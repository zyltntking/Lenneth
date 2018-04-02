using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB
{
    internal class KeyDocument
    {
        public BsonValue Key { get; set; }
        public BsonDocument Document { get; set; }
    }

    internal class KeyDocumentComparer : IComparer<KeyDocument>
    {
        public int Compare(KeyDocument x, KeyDocument y)
        {
            return x.Key.CompareTo(y.Key);
        }

        public int GetHashCode(KeyDocument obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
