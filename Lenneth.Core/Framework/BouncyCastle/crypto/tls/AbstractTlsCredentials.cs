namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public abstract class AbstractTlsCredentials
        :   TlsCredentials
    {
        public abstract Certificate Certificate { get; }
    }
}
