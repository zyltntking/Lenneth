using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public class AbstractTlsCipherFactory
        :   TlsCipherFactory
    {
        /// <exception cref="IOException"></exception>
        public virtual TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
        {
            throw new TlsFatalAlert(AlertDescription.internal_error);
        }
    }
}
