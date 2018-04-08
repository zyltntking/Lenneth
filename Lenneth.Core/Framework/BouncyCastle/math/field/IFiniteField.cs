namespace Lenneth.Core.FrameWork.BouncyCastle.math.field
{
    public interface IFiniteField
    {
        BigInteger Characteristic { get; }

        int Dimension { get; }
    }
}
