using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cryptopro;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.nist;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.oiw;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.pkcs;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.teletrust;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x9;
using Lenneth.Core.FrameWork.BouncyCastle.crypto;

namespace Lenneth.Core.FrameWork.BouncyCastle.x509
{
	internal class X509SignatureUtilities
	{
		private static readonly Asn1Null derNull = DerNull.Instance;

		internal static void SetSignatureParameters(
			ISigner			signature,
			Asn1Encodable	parameters)
		{
			if (parameters != null && !derNull.Equals(parameters))
			{
				// TODO Put back in
//				AlgorithmParameters sigParams = AlgorithmParameters.GetInstance(signature.getAlgorithm());
//
//				try
//				{
//					sigParams.Init(parameters.ToAsn1Object().GetDerEncoded());
//				}
//				catch (IOException e)
//				{
//					throw new SignatureException("IOException decoding parameters: " + e.Message);
//				}
//
//				if (Platform.EndsWith(signature.getAlgorithm(), "MGF1"))
//				{
//					try
//					{
//						signature.setParameter(sigParams.getParameterSpec(PSSParameterSpec.class));
//					}
//					catch (GeneralSecurityException e)
//					{
//						throw new SignatureException("Exception extracting parameters: " + e.Message);
//					}
//				}
			}
		}

		internal static string GetSignatureName(
			AlgorithmIdentifier sigAlgId)
		{
			Asn1Encodable parameters = sigAlgId.Parameters;

			if (parameters != null && !derNull.Equals(parameters))
			{
                if (sigAlgId.Algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss))
				{
					RsassaPssParameters rsaParams = RsassaPssParameters.GetInstance(parameters);

                    return GetDigestAlgName(rsaParams.HashAlgorithm.Algorithm) + "withRSAandMGF1";
				}
                if (sigAlgId.Algorithm.Equals(X9ObjectIdentifiers.ECDsaWithSha2))
				{
					Asn1Sequence ecDsaParams = Asn1Sequence.GetInstance(parameters);

					return GetDigestAlgName((DerObjectIdentifier)ecDsaParams[0]) + "withECDSA";
				}
			}

            return sigAlgId.Algorithm.Id;
		}

		/**
		 * Return the digest algorithm using one of the standard JCA string
		 * representations rather than the algorithm identifier (if possible).
		 */
		private static string GetDigestAlgName(
			DerObjectIdentifier digestAlgOID)
		{
			if (PkcsObjectIdentifiers.MD5.Equals(digestAlgOID))
			{
				return "MD5";
			}
			else if (OiwObjectIdentifiers.IdSha1.Equals(digestAlgOID))
			{
				return "SHA1";
			}
			else if (NistObjectIdentifiers.IdSha224.Equals(digestAlgOID))
			{
				return "SHA224";
			}
			else if (NistObjectIdentifiers.IdSha256.Equals(digestAlgOID))
			{
				return "SHA256";
			}
			else if (NistObjectIdentifiers.IdSha384.Equals(digestAlgOID))
			{
				return "SHA384";
			}
			else if (NistObjectIdentifiers.IdSha512.Equals(digestAlgOID))
			{
				return "SHA512";
			}
			else if (TeleTrusTObjectIdentifiers.RipeMD128.Equals(digestAlgOID))
			{
				return "RIPEMD128";
			}
			else if (TeleTrusTObjectIdentifiers.RipeMD160.Equals(digestAlgOID))
			{
				return "RIPEMD160";
			}
			else if (TeleTrusTObjectIdentifiers.RipeMD256.Equals(digestAlgOID))
			{
				return "RIPEMD256";
			}
			else if (CryptoProObjectIdentifiers.GostR3411.Equals(digestAlgOID))
			{
				return "GOST3411";
			}
			else
			{
				return digestAlgOID.Id;
			}
		}
	}
}
