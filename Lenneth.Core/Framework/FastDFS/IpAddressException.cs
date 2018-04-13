using System;

namespace Lenneth.Core.Framework.FastDFS
{
    internal class IpAddressException : Exception
    {
        public IpAddressException() : base("错误的IP地址")
        {
        }
    }
}