using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;

namespace Lenneth.Core.FrameWork.BouncyCastle.asn1.pkcs
{
	public class KeyDerivationFunc
		: AlgorithmIdentifier
	{
		internal KeyDerivationFunc(Asn1Sequence seq)
			: base(seq)
		{
		}

		public KeyDerivationFunc(
			DerObjectIdentifier	id,
			Asn1Encodable		parameters)
			: base(id, parameters)
		{
		}
	}
}