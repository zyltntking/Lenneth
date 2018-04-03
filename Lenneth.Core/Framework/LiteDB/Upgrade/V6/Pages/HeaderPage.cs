using System;
using System.Collections.Generic;
using Lenneth.Core.Framework.LiteDB;

namespace Lenneth.Core.Framework.LiteDB_V6
{
    internal class HeaderPage : BasePage
    {
        private const string HEADER_INFO = "** This is a LiteDB file **";
        private const byte FILE_VERSION = 6;

        public override PageType PageType { get { return PageType.Header; } }
        public ushort ChangeID { get; set; }
        public uint FreeEmptyPageID;
        public uint LastPageID { get; set; }
        public ushort DbVersion = 0;
        public byte[] Password = new byte[20];
        public Dictionary<string, uint> CollectionPages { get; set; }

        public HeaderPage()
            : base(0)
        {
            FreeEmptyPageID = uint.MaxValue;
            ChangeID = 0;
            LastPageID = 0;
            ItemCount = 1; // fixed for header
            DbVersion = 0;
            Password = new byte[20];
            CollectionPages = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
        }

        protected override void ReadContent(ByteReader reader)
        {
            var info = reader.ReadString(HEADER_INFO.Length);
            var ver = reader.ReadByte();

            ChangeID = reader.ReadUInt16();
            FreeEmptyPageID = reader.ReadUInt32();
            LastPageID = reader.ReadUInt32();
            DbVersion = reader.ReadUInt16();
            Password = reader.ReadBytes(Password.Length);

            // read page collections references (position on end of page)
            var cols = reader.ReadByte();
            for (var i = 0; i < cols; i++)
            {
                CollectionPages.Add(reader.ReadString(), reader.ReadUInt32());
            }
        }
    }
}