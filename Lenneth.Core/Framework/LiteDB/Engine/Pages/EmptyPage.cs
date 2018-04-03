using System;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// Represent a empty page (reused)
    /// </summary>
    internal class EmptyPage : BasePage
    {
        /// <summary>
        /// Page type = Empty
        /// </summary>
        public override PageType PageType { get { return PageType.Empty; } }

        public EmptyPage(uint pageID)
            : base(pageID)
        {
            ItemCount = 0;
            FreeBytes = PAGE_AVAILABLE_BYTES;
        }

        public EmptyPage(BasePage page)
            : base(page.PageID)
        {
            if(page.DiskData.Length > 0)
            {
                DiskData = new byte[PAGE_SIZE];
                Buffer.BlockCopy(page.DiskData, 0, DiskData, 0, PAGE_SIZE);
            }
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
        }

        protected override void WriteContent(ByteWriter writer)
        {
        }

        #endregion
    }
}