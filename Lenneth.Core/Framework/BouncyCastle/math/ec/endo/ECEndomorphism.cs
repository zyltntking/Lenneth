namespace Lenneth.Core.FrameWork.BouncyCastle.math.ec.endo
{
    public interface ECEndomorphism
    {
        ECPointMap PointMap { get; }

        bool HasEfficientPointMap { get; }
    }
}
