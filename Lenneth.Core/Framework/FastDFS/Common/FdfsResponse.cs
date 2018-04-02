using System.IO;

namespace Lenneth.Core.Framework.FastDFS.Common
{

    public class FdfsResponse
    {
        public virtual void ReceiveResponse(Stream stream, long length)
        {
            var content = new byte[length];
            stream.Read(content, 0, (int) length);
            LoadContent(content);
        }

        protected virtual void LoadContent(byte[] content)
        {
        }
    }
}