namespace Lenneth.Core.FrameWork.BouncyCastle.math.ec.endo
{
    public interface GlvEndomorphism
        :   ECEndomorphism
    {
        BigInteger[] DecomposeScalar(BigInteger k);
    }
}
