using Lenneth.Core.FrameWork.BouncyCastle.math;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1.x509
{
    /**
     * The CRLNumber object.
     * <pre>
     * CRLNumber::= Integer(0..MAX)
     * </pre>
     */
    public class CrlNumber
        : DerInteger
    {
        public CrlNumber(
			BigInteger number)
			: base(number)
        {
        }

		public BigInteger Number
		{
			get { return PositiveValue; }
		}

		public override string ToString()
		{
			return "CRLNumber: " + Number;
		}
	}
}
