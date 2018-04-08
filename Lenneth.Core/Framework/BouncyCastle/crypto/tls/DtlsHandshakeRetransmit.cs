using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    interface DtlsHandshakeRetransmit
    {
        /// <exception cref="IOException"/>
        void ReceivedHandshakeRecord(int epoch, byte[] buf, int off, int len);
    }
}
