using Lenneth.Core.FrameWork.BouncyCastle.crypto.parameters;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public interface TlsSrpGroupVerifier
    {
        /**
         * Check whether the given SRP group parameters are acceptable for use.
         * 
         * @param group the {@link SRP6GroupParameters} to check
         * @return true if (and only if) the specified group parameters are acceptable
         */
        bool Accept(Srp6GroupParameters group);
    }
}
