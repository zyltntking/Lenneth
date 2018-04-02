using System;

namespace Lenneth.Core.Framework.FastDFS.Common
{
    public class FdfsException : Exception
    {
        public FdfsException(string msg) : base(msg)
        {
        }
        public int ErrorCode { get; set; }
    }
}