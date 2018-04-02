using System.Collections.Generic;

namespace Lenneth.Core.Framework.FastDFS.Config
{
    public abstract class FastDfsConfig
    {
        protected FastDfsConfig()
        {
            FastDfsServer = new List<FastDfsServer>();
        }

        public string GroupName { get; set; }

        public List<FastDfsServer> FastDfsServer { get; set; }
    }
}