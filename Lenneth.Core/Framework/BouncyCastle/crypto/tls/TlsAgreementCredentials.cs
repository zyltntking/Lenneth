using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public interface TlsAgreementCredentials
        :   TlsCredentials
    {
        /// <exception cref="IOException"></exception>
        byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
    }
}
