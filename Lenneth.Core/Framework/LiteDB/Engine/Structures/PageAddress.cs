namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// Represents a page address inside a page structure - index could be byte offset position OR index in a list (6 bytes)
    /// </summary>
    internal struct PageAddress
    {
        public const int SIZE = 6;

        public static PageAddress Empty = new PageAddress(uint.MaxValue, ushort.MaxValue);

        /// <summary>
        /// PageID (4 bytes)
        /// </summary>
        public uint PageID;

        /// <summary>
        /// Index inside page (2 bytes)
        /// </summary>
        public ushort Index;

        public bool IsEmpty
        {
            get { return PageID == uint.MaxValue; }
        }

        public override bool Equals(object obj)
        {
            var other = (PageAddress)obj;

            return PageID == other.PageID && Index == other.Index;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                // Maybe nullity checks, if these are objects not primitives!
                hash = hash * 23 + (int)PageID;
                hash = hash * 23 + Index;
                return hash;
            }
        }

        public PageAddress(uint pageID, ushort index)
        {
            PageID = pageID;
            Index = index;
        }

        public override string ToString()
        {
            return IsEmpty ? "----" : PageID.ToString() + ":" + Index.ToString();
        }
    }
}