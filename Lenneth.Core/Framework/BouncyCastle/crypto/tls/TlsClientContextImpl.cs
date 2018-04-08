using Lenneth.Core.FrameWork.BouncyCastle.security;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    internal class TlsClientContextImpl
        :   AbstractTlsContext, TlsClientContext
    {
        internal TlsClientContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters)
            :   base(secureRandom, securityParameters)
        {
        }

        public override bool IsServer
        {
            get { return false; }
        }
    }
}
