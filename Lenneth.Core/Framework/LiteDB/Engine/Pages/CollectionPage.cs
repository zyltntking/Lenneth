using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// Represents the collection page AND a collection item, because CollectionPage represent a Collection (1 page = 1 collection). All collections pages are linked with Prev/Next links
    /// </summary>
    internal class CollectionPage : BasePage
    {
        /// <summary>
        /// Represent maximum bytes that all collections names can be used in header
        /// </summary>
        public const ushort MAX_COLLECTIONS_SIZE = 3000;

        public static Regex NamePattern = new Regex(@"^[\w-]{1,60}$", RegexOptions.Compiled);

        /// <summary>
        /// Page type = Collection
        /// </summary>
        public override PageType PageType { get { return PageType.Collection; } }

        /// <summary>
        /// Name of collection
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Get a reference for the free list data page - its private list per collection - each DataPage contains only data for 1 collection (no mixing)
        /// Must to be a Field to be used as parameter reference
        /// </summary>
        public uint FreeDataPageID;

        /// <summary>
        /// Get the number of documents inside this collection
        /// </summary>
        public long DocumentCount { get; set; }

        /// <summary>
        /// Get all indexes from this collection - includes non-used indexes
        /// </summary>
        public CollectionIndex[] Indexes { get; set; }

        /// <summary>
        /// Storage number sequence to be used in auto _id values
        /// </summary>
        public long Sequence { get; set; }

        public CollectionPage(uint pageID)
            : base(pageID)
        {
            FreeDataPageID = uint.MaxValue;
            DocumentCount = 0;
            ItemCount = 1; // fixed for CollectionPage
            FreeBytes = 0; // no free bytes on collection-page - only one collection per page
            Indexes = new CollectionIndex[CollectionIndex.INDEX_PER_COLLECTION];
            Sequence = 0;

            for (var i = 0; i < Indexes.Length; i++)
            {
                Indexes[i] = new CollectionIndex() { Page = this, Slot = i };
            }
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            CollectionName = reader.ReadString();
            DocumentCount = reader.ReadInt64();
            FreeDataPageID = reader.ReadUInt32();

            foreach (var index in Indexes)
            {
                var field = reader.ReadString();
                var eq = field.IndexOf('=');

                // Use same string to avoid change file defition
                if (eq > 0)
                {
                    index.Field = field.Substring(0, eq);
                    index.Expression = field.Substring(eq + 1);
                }
                else
                {
                    index.Field = field;
                    index.Expression = "$." + field;
                }

                index.Unique = reader.ReadBoolean();
                index.HeadNode = reader.ReadPageAddress();
                index.TailNode = reader.ReadPageAddress();
                index.FreeIndexPageID = reader.ReadUInt32();
            }

            // position on page-footer (avoid file structure change)
            reader.Position = PAGE_SIZE - 8 - CollectionIndex.INDEX_PER_COLLECTION;

            foreach (var index in Indexes)
            {
                var maxLevel = reader.ReadByte();
                index.MaxLevel = maxLevel == 0 ? (byte)IndexNode.MAX_LEVEL_LENGTH : maxLevel;
            }

            Sequence = reader.ReadInt64();
        }

        protected override void WriteContent(ByteWriter writer)
        {
            writer.Write(CollectionName);
            writer.Write(DocumentCount);
            writer.Write(FreeDataPageID);

            foreach (var index in Indexes)
            {
                // write Field+Expression only if index are used
                if(index.Field.Length > 0)
                {
                    writer.Write(index.Field + "=" + index.Expression);
                }
                else
                {
                    writer.Write("");
                }

                writer.Write(index.Unique);
                writer.Write(index.HeadNode);
                writer.Write(index.TailNode);
                writer.Write(index.FreeIndexPageID);
            }

            // position on page-footer (avoid file structure change)
            writer.Position = PAGE_SIZE - 8 - CollectionIndex.INDEX_PER_COLLECTION;

            foreach (var index in Indexes)
            {
                writer.Write(index.MaxLevel);
            }

            writer.Write(Sequence);
        }

        #endregion

        #region Methods to work with index array

        /// <summary>
        /// Returns first free index slot to be used
        /// </summary>
        public CollectionIndex GetFreeIndex()
        {
            for (byte i = 0; i < Indexes.Length; i++)
            {
                if (Indexes[i].IsEmpty) return Indexes[i];
            }

            throw LiteException.IndexLimitExceeded(CollectionName);
        }

        /// <summary>
        /// Get index from field name (index field name is case sensitive) - returns null if not found
        /// </summary>
        public CollectionIndex GetIndex(string field)
        {
            return Indexes.FirstOrDefault(x => x.Field == field);
        }

        /// <summary>
        /// Get primary key index (_id index)
        /// </summary>
        public CollectionIndex PK { get { return Indexes[0]; } }

        /// <summary>
        /// Returns all used indexes
        /// </summary>
        public IEnumerable<CollectionIndex> GetIndexes(bool includePK)
        {
            return Indexes.Where(x => x.IsEmpty == false && x.Slot >= (includePK ? 0 : 1));
        }

        #endregion
    }
}