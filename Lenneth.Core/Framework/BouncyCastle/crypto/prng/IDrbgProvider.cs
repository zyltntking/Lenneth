using Lenneth.Core.FrameWork.BouncyCastle.crypto.prng.drbg;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.prng
{
    internal interface IDrbgProvider
    {
        ISP80090Drbg Get(IEntropySource entropySource);
    }
}
