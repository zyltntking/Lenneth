using System;
using Lenneth.Core.FrameWork.BouncyCastle.math;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.parameters
{
    public class ElGamalPublicKeyParameters
		: ElGamalKeyParameters
    {
        private readonly BigInteger y;

		public ElGamalPublicKeyParameters(
            BigInteger			y,
            ElGamalParameters	parameters)
			: base(false, parameters)
        {
			if (y == null)
				throw new ArgumentNullException("y");

			this.y = y;
        }

		public BigInteger Y
        {
            get { return y; }
        }

		public override bool Equals(
            object obj)
        {
			if (obj == this)
				return true;

			ElGamalPublicKeyParameters other = obj as ElGamalPublicKeyParameters;

			if (other == null)
				return false;

			return Equals(other);
        }

		protected bool Equals(
			ElGamalPublicKeyParameters other)
		{
			return y.Equals(other.y) && base.Equals(other);
		}

		public override int GetHashCode()
        {
			return y.GetHashCode() ^ base.GetHashCode();
        }
    }
}
