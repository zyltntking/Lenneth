using System;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x9;
using Lenneth.Core.FrameWork.BouncyCastle.crypto.agreement.kdf;
using Lenneth.Core.FrameWork.BouncyCastle.math;
using Lenneth.Core.FrameWork.BouncyCastle.security;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.agreement
{
	public class ECDHWithKdfBasicAgreement
		: ECDHBasicAgreement
	{
		private readonly string algorithm;
		private readonly IDerivationFunction kdf;

		public ECDHWithKdfBasicAgreement(
			string				algorithm,
			IDerivationFunction	kdf)
		{
			if (algorithm == null)
				throw new ArgumentNullException("algorithm");
			if (kdf == null)
				throw new ArgumentNullException("kdf");

			this.algorithm = algorithm;
			this.kdf = kdf;
		}

		public override BigInteger CalculateAgreement(
			ICipherParameters pubKey)
		{
			// Note that the ec.KeyAgreement class in JCE only uses kdf in one
			// of the engineGenerateSecret methods.

			BigInteger result = base.CalculateAgreement(pubKey);

			int keySize = GeneratorUtilities.GetDefaultKeySize(algorithm);

			DHKdfParameters dhKdfParams = new DHKdfParameters(
				new DerObjectIdentifier(algorithm),
				keySize,
				BigIntToBytes(result));

			kdf.Init(dhKdfParams);

			byte[] keyBytes = new byte[keySize / 8];
			kdf.GenerateBytes(keyBytes, 0, keyBytes.Length);

			return new BigInteger(1, keyBytes);
		}

		private byte[] BigIntToBytes(BigInteger r)
		{
			int byteLength = X9IntegerConverter.GetByteLength(privKey.Parameters.Curve);
			return X9IntegerConverter.IntegerToBytes(r, byteLength);
		}
	}
}
