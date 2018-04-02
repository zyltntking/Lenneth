using System.Net;

namespace Lenneth.Core.Framework.FastDFS.Common
{
    public class StorageNode
    {
        public string GroupName { get; internal set; }
        public IPEndPoint EndPoint { get; internal set; }
        public byte StorePathIndex { get; internal set; }
    }
}