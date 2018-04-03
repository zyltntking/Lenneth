using System.Text.RegularExpressions;
using Lenneth.Core.Framework.LiteDB;

namespace Lenneth.Core.Framework.LiteDB_V6
{
    internal class CollectionIndex
    {
        public static Regex IndexPattern = new Regex(@"[\w-$\.]+$", RegexOptions.Compiled);

        /// <summary>
        /// Total indexes per collection - it's fixed because I will used fixed arrays allocations
        /// </summary>
        public const int INDEX_PER_COLLECTION = 16;

        /// <summary>
        /// Represent slot position on index array on dataBlock/collection indexes - non-persistable
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Field name
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Unique index
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Head page address for this index
        /// </summary>
        public PageAddress HeadNode { get; set; }

        /// <summary>
        /// A link pointer to tail node
        /// </summary>
        public PageAddress TailNode { get; set; }

        /// <summary>
        /// Get a reference for the free list index page - its private list per collection/index (must be a Field to be used as reference parameter)
        /// </summary>
        public uint FreeIndexPageID;

        /// <summary>
        /// Get a reference for page
        /// </summary>
        public CollectionPage Page { get; set; }

        public CollectionIndex()
        {
            Field = string.Empty;
            Unique = false;
            HeadNode = PageAddress.Empty;
            FreeIndexPageID = uint.MaxValue;
        }
    }
}