using System;
using System.Collections.Generic;
using Lenneth.Core.Framework.LiteDB;
using IndexNode = Lenneth.Core.Framework.LiteDB_V6.IndexNode;

namespace Lenneth.Core.Framework.LiteDB_V6
{
    internal class IndexPage : BasePage
    {
        public override PageType PageType { get { return PageType.Index; } }
        public Dictionary<ushort, IndexNode> Nodes { get; set; }

        public IndexPage(uint pageID)
            : base(pageID)
        {
            Nodes = new Dictionary<ushort, IndexNode>();
        }
		
        protected override void ReadContent(ByteReader reader)
        {
            Nodes = new Dictionary<ushort, IndexNode>(ItemCount);

            for (var i = 0; i < ItemCount; i++)
            {
                var index = reader.ReadUInt16();
                var levels = reader.ReadByte();

                var node = new IndexNode(levels);

                node.Page = this;
                node.Position = new PageAddress(PageID, index);
                node.KeyLength = reader.ReadUInt16();
                node.Key = ReadBsonValue(reader, node.KeyLength);
                node.DataBlock = reader.ReadPageAddress();

                for (var j = 0; j < node.Prev.Length; j++)
                {
                    node.Prev[j] = reader.ReadPageAddress();
                    node.Next[j] = reader.ReadPageAddress();
                }

                Nodes.Add(node.Position.Index, node);
            }
        }
        /// <summary>
        /// Write a custom ReadBsonValue because BsonType changed from v6 to v7
        /// </summary>
        private BsonValue ReadBsonValue(ByteReader reader, ushort length)
        {
            var type = reader.ReadByte();

            // using numbers because BsonType changed
            switch (type)
            {
                case 1: return BsonValue.Null;

                case 2: return reader.ReadInt32();
                case 3: return reader.ReadInt64();
                case 4: return reader.ReadDouble();

                case 5: return reader.ReadString(length);

                case 6: return new BsonReader(false).ReadDocument(reader);
                case 7: return new BsonReader(false).ReadArray(reader);

                case 8: return reader.ReadBytes(length);
                case 9: return reader.ReadObjectId();
                case 10: return reader.ReadGuid();

                case 11: return reader.ReadBoolean();
                case 12: return reader.ReadDateTime();

                case 0: return BsonValue.MinValue;
                case 13: return BsonValue.MaxValue;
            }

            throw new NotImplementedException();
        }
    }
}