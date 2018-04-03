using System;
using System.Collections.Generic;

namespace Lenneth.Core.Framework.LiteDB
{
    internal class HeaderPage : BasePage
    {
        /// <summary>
        /// Page type = Header
        /// </summary>
        public override PageType PageType { get { return PageType.Header; } }

        /// <summary>
        /// Header info the validate that datafile is a LiteDB file (27 bytes)
        /// </summary>
        private const string HEADER_INFO = "** This is a LiteDB file **";

        /// <summary>
        /// Datafile specification version
        /// </summary>
        private const byte FILE_VERSION = 7;

        /// <summary>
        /// Last modified transaction. Used to detect when other process change datafile and cache are not valid anymore
        /// </summary>
        public ushort ChangeID { get; set; }

        /// <summary>
        /// Get/Set the pageID that start sequence with a complete empty pages (can be used as a new page)
        /// Must be a field to be used as "ref"
        /// </summary>
        public uint FreeEmptyPageID;

        /// <summary>
        /// Last created page - Used when there is no free page inside file
        /// </summary>
        public uint LastPageID { get; set; }

        /// <summary>
        /// Database user version [2 bytes]
        /// </summary>
        public ushort UserVersion { get; set; }

        /// <summary>
        /// Password hash in SHA1 [20 bytes]
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        /// When using encryption, store salt for password
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Indicate if datafile need be recovered
        /// </summary>
        public bool Recovery { get; set; }

        /// <summary>
        /// Get a dictionary with all collection pages with pageID link
        /// </summary>
        public Dictionary<string, uint> CollectionPages { get; set; }

        public HeaderPage()
            : base(0)
        {
            ChangeID = 0;
            FreeEmptyPageID = uint.MaxValue;
            LastPageID = 0;
            ItemCount = 1; // fixed for header
            FreeBytes = 0; // no free bytes on header
            UserVersion = 0;
            Password = new byte[20];
            Salt = new byte[16];
            CollectionPages = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
        }

        #region Read/Write pages

        protected override void ReadContent(ByteReader reader)
        {
            var info = reader.ReadString(HEADER_INFO.Length);
            var ver = reader.ReadByte();

            if (info != HEADER_INFO) throw LiteException.InvalidDatabase();
            if (ver != FILE_VERSION) throw LiteException.InvalidDatabaseVersion(ver);

            ChangeID = reader.ReadUInt16();
            FreeEmptyPageID = reader.ReadUInt32();
            LastPageID = reader.ReadUInt32();
            UserVersion = reader.ReadUInt16();
            Password = reader.ReadBytes(Password.Length);
            Salt = reader.ReadBytes(Salt.Length);

            // read page collections references (position on end of page)
            var cols = reader.ReadByte();
            for (var i = 0; i < cols; i++)
            {
                CollectionPages.Add(reader.ReadString(), reader.ReadUInt32());
            }

            // use last page byte position for recovery mode only because i forgot to reserve area before collection names!
            // TODO: fix this in next change data structure
            reader.Position = PAGE_SIZE - 1;
            Recovery = reader.ReadBoolean();
        }

        protected override void WriteContent(ByteWriter writer)
        {
            writer.Write(HEADER_INFO, HEADER_INFO.Length);
            writer.Write(FILE_VERSION);
            writer.Write(ChangeID);
            writer.Write(FreeEmptyPageID);
            writer.Write(LastPageID);
            writer.Write(UserVersion);
            writer.Write(Password);
            writer.Write(Salt);

            writer.Write((byte)CollectionPages.Count);
            foreach (var key in CollectionPages.Keys)
            {
                writer.Write(key);
                writer.Write(CollectionPages[key]);
            }

            writer.Position = PAGE_SIZE - 1;
            writer.Write(Recovery);
        }

        #endregion
    }
}