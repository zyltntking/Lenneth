namespace Lenneth.Core.FrameWork.BouncyCastle.math.field
{
    public interface IPolynomialExtensionField
        : IExtensionField
    {
        IPolynomial MinimalPolynomial { get; }
    }
}
