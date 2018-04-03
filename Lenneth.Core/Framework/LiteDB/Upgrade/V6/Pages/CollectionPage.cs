using System;
using Lenneth.Core.Framework.LiteDB;

namespace Lenneth.Core.Framework.LiteDB_V6
{
    /// <summary>
    /// Represents the collection page AND a collection item, because CollectionPage represent a Collection (1 page = 1 collection). All collections pages are linked with Prev/Next links
    /// </summary>
    internal class CollectionPage : BasePage
    {
        public override PageType PageType { get { return PageType.Collection; } }
        public string CollectionName { get; set; }
        public uint FreeDataPageID;
        public long DocumentCount { get; set; }
        public CollectionIndex[] Indexes { get; set; }

        public CollectionPage(uint pageID)
            : base(pageID)
        {
            FreeDataPageID = uint.MaxValue;
            DocumentCount = 0;
            ItemCount = 1; // fixed for CollectionPage
            Indexes = new CollectionIndex[CollectionIndex.INDEX_PER_COLLECTION];

            for (var i = 0; i < Indexes.Length; i++)
            {
                Indexes[i] = new CollectionIndex() { Page = this, Slot = i };
            }
        }

        protected override void ReadContent(ByteReader reader)
        {
            CollectionName = reader.ReadString();
            FreeDataPageID = reader.ReadUInt32();
            var uintCount = reader.ReadUInt32(); // read as uint (4 bytes)

            foreach (var index in Indexes)
            {
                index.Field = reader.ReadString();
                index.HeadNode = reader.ReadPageAddress();
                index.TailNode = reader.ReadPageAddress();
                index.FreeIndexPageID = reader.ReadUInt32();
                index.Unique = reader.ReadBoolean();
                reader.ReadBoolean(); // IgnoreCase
                reader.ReadBoolean(); // TrimWhitespace
                reader.ReadBoolean(); // EmptyStringToNull
                reader.ReadBoolean(); // RemoveAccents
            }

            // be compatible with v2_beta
            var longCount = reader.ReadInt64();
            DocumentCount = Math.Max(uintCount, longCount);
        }

        /// <summary>
        /// Get primary key index (_id index)
        /// </summary>
        public CollectionIndex PK { get { return Indexes[0]; } }
    }
}