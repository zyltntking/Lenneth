﻿using Lenneth.Core.Framework.LiteDB;

namespace Lenneth.Core.Framework.LiteDB_V6
{
    internal class DataBlock
    {
        /// <summary>
        /// Position of this dataBlock inside a page (store only Position.Index)
        /// </summary>
        public PageAddress Position { get; set; }

        /// <summary>
        /// Indexes nodes for all indexes for this data block
        /// </summary>
        public PageAddress[] IndexRef { get; set; }

        /// <summary>
        /// If object is bigger than this page - use a ExtendPage (and do not use Data array)
        /// </summary>
        public uint ExtendPageID { get; set; }

        /// <summary>
        /// Data of a record - could be empty if is used in ExtedPage
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Get a reference for page
        /// </summary>
        public DataPage Page { get; set; }

        public DataBlock()
        {
            Position = PageAddress.Empty;
            ExtendPageID = uint.MaxValue;
            Data = new byte[0];

            IndexRef = new PageAddress[CollectionIndex.INDEX_PER_COLLECTION];

            for (var i = 0; i < CollectionIndex.INDEX_PER_COLLECTION; i++)
            {
                IndexRef[i] = PageAddress.Empty;
            }
        }
    }
}