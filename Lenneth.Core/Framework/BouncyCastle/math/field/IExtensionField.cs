namespace Lenneth.Core.FrameWork.BouncyCastle.math.field
{
    public interface IExtensionField
        : IFiniteField
    {
        IFiniteField Subfield { get; }

        int Degree { get; }
    }
}
