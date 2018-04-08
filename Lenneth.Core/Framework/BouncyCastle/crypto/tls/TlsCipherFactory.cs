using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public interface TlsCipherFactory
    {
        /// <exception cref="IOException"></exception>
        TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm);
    }
}
