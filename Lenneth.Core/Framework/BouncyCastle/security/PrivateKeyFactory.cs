using System;
using System.IO;
using Lenneth.Core.FrameWork.BouncyCastle.asn1;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.cryptopro;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.oiw;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.pkcs;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.sec;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x509;
using Lenneth.Core.FrameWork.BouncyCastle.asn1.x9;
using Lenneth.Core.FrameWork.BouncyCastle.crypto;
using Lenneth.Core.FrameWork.BouncyCastle.crypto.generators;
using Lenneth.Core.FrameWork.BouncyCastle.crypto.parameters;
using Lenneth.Core.FrameWork.BouncyCastle.math;
using Lenneth.Core.FrameWork.BouncyCastle.pkcs;
using Lenneth.Core.FrameWork.BouncyCastle.util;

namespace Lenneth.Core.FrameWork.BouncyCastle.security
{
    public sealed class PrivateKeyFactory
    {
        private PrivateKeyFactory()
        {
        }

        public static AsymmetricKeyParameter CreateKey(
            byte[] privateKeyInfoData)
        {
            return CreateKey(
                PrivateKeyInfo.GetInstance(
                    Asn1Object.FromByteArray(privateKeyInfoData)));
        }

        public static AsymmetricKeyParameter CreateKey(
            Stream inStr)
        {
            return CreateKey(
                PrivateKeyInfo.GetInstance(
                    Asn1Object.FromStream(inStr)));
        }

        public static AsymmetricKeyParameter CreateKey(
            PrivateKeyInfo keyInfo)
        {
            AlgorithmIdentifier algID = keyInfo.PrivateKeyAlgorithm;
            DerObjectIdentifier algOid = algID.Algorithm;

            // TODO See RSAUtil.isRsaOid in Java build
            if (algOid.Equals(PkcsObjectIdentifiers.RsaEncryption)
                || algOid.Equals(X509ObjectIdentifiers.IdEARsa)
                || algOid.Equals(PkcsObjectIdentifiers.IdRsassaPss)
                || algOid.Equals(PkcsObjectIdentifiers.IdRsaesOaep))
            {
                RsaPrivateKeyStructure keyStructure = RsaPrivateKeyStructure.GetInstance(keyInfo.ParsePrivateKey());

                return new RsaPrivateCrtKeyParameters(
                    keyStructure.Modulus,
                    keyStructure.PublicExponent,
                    keyStructure.PrivateExponent,
                    keyStructure.Prime1,
                    keyStructure.Prime2,
                    keyStructure.Exponent1,
                    keyStructure.Exponent2,
                    keyStructure.Coefficient);
            }
            // TODO?
//			else if (algOid.Equals(X9ObjectIdentifiers.DHPublicNumber))
            else if (algOid.Equals(PkcsObjectIdentifiers.DhKeyAgreement))
            {
                DHParameter para = new DHParameter(
                    Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));
                DerInteger derX = (DerInteger)keyInfo.ParsePrivateKey();

                BigInteger lVal = para.L;
                int l = lVal == null ? 0 : lVal.IntValue;
                DHParameters dhParams = new DHParameters(para.P, para.G, null, l);

                return new DHPrivateKeyParameters(derX.Value, dhParams, algOid);
            }
            else if (algOid.Equals(OiwObjectIdentifiers.ElGamalAlgorithm))
            {
                ElGamalParameter  para = new ElGamalParameter(
                    Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));
                DerInteger derX = (DerInteger)keyInfo.ParsePrivateKey();

                return new ElGamalPrivateKeyParameters(
                    derX.Value,
                    new ElGamalParameters(para.P, para.G));
            }
            else if (algOid.Equals(X9ObjectIdentifiers.IdDsa))
            {
                DerInteger derX = (DerInteger)keyInfo.ParsePrivateKey();
                Asn1Encodable ae = algID.Parameters;

                DsaParameters parameters = null;
                if (ae != null)
                {
                    DsaParameter para = DsaParameter.GetInstance(ae.ToAsn1Object());
                    parameters = new DsaParameters(para.P, para.Q, para.G);
                }

                return new DsaPrivateKeyParameters(derX.Value, parameters);
            }
            else if (algOid.Equals(X9ObjectIdentifiers.IdECPublicKey))
            {
                X962Parameters para = new X962Parameters(algID.Parameters.ToAsn1Object());

                X9ECParameters x9;
                if (para.IsNamedCurve)
                {
                    x9 = ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier)para.Parameters);
                }
                else
                {
                    x9 = new X9ECParameters((Asn1Sequence)para.Parameters);
                }

                ECPrivateKeyStructure ec = ECPrivateKeyStructure.GetInstance(keyInfo.ParsePrivateKey());
                BigInteger d = ec.GetKey();

                if (para.IsNamedCurve)
                {
                    return new ECPrivateKeyParameters("EC", d, (DerObjectIdentifier)para.Parameters);
                }

                ECDomainParameters dParams = new ECDomainParameters(x9.Curve, x9.G, x9.N, x9.H,  x9.GetSeed());
                return new ECPrivateKeyParameters(d, dParams);
            }
            else if (algOid.Equals(CryptoProObjectIdentifiers.GostR3410x2001))
            {
                Gost3410PublicKeyAlgParameters gostParams = new Gost3410PublicKeyAlgParameters(
                    Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));

                ECDomainParameters ecP = ECGost3410NamedCurves.GetByOid(gostParams.PublicKeyParamSet);

                if (ecP == null)
                    throw new ArgumentException("Unrecognized curve OID for GostR3410x2001 private key");

                Asn1Object privKey = keyInfo.ParsePrivateKey();
                ECPrivateKeyStructure ec;

                if (privKey is DerInteger)
                {
                    // TODO Do we need to pass any parameters here?
                    ec = new ECPrivateKeyStructure(ecP.N.BitLength, ((DerInteger)privKey).Value);
                }
                else
                {
                    ec = ECPrivateKeyStructure.GetInstance(privKey);
                }

                return new ECPrivateKeyParameters("ECGOST3410", ec.GetKey(), gostParams.PublicKeyParamSet);
            }
            else if (algOid.Equals(CryptoProObjectIdentifiers.GostR3410x94))
            {
                Gost3410PublicKeyAlgParameters gostParams = new Gost3410PublicKeyAlgParameters(
                    Asn1Sequence.GetInstance(algID.Parameters.ToAsn1Object()));

                DerOctetString derX = (DerOctetString)keyInfo.ParsePrivateKey();
                BigInteger x = new BigInteger(1, Arrays.Reverse(derX.GetOctets()));

                return new Gost3410PrivateKeyParameters(x, gostParams.PublicKeyParamSet);
            }
            else
            {
                throw new SecurityUtilityException("algorithm identifier in key not recognised");
            }
        }

        public static AsymmetricKeyParameter DecryptKey(
            char[]					passPhrase,
            EncryptedPrivateKeyInfo	encInfo)
        {
            return CreateKey(PrivateKeyInfoFactory.CreatePrivateKeyInfo(passPhrase, encInfo));
        }

        public static AsymmetricKeyParameter DecryptKey(
            char[]	passPhrase,
            byte[]	encryptedPrivateKeyInfoData)
        {
            return DecryptKey(passPhrase, Asn1Object.FromByteArray(encryptedPrivateKeyInfoData));
        }

        public static AsymmetricKeyParameter DecryptKey(
            char[]	passPhrase,
            Stream	encryptedPrivateKeyInfoStream)
        {
            return DecryptKey(passPhrase, Asn1Object.FromStream(encryptedPrivateKeyInfoStream));
        }

        private static AsymmetricKeyParameter DecryptKey(
            char[]		passPhrase,
            Asn1Object	asn1Object)
        {
            return DecryptKey(passPhrase, EncryptedPrivateKeyInfo.GetInstance(asn1Object));
        }

        public static byte[] EncryptKey(
            DerObjectIdentifier		algorithm,
            char[]					passPhrase,
            byte[]					salt,
            int						iterationCount,
            AsymmetricKeyParameter	key)
        {
            return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(
                algorithm, passPhrase, salt, iterationCount, key).GetEncoded();
        }

        public static byte[] EncryptKey(
            string					algorithm,
            char[]					passPhrase,
            byte[]					salt,
            int						iterationCount,
            AsymmetricKeyParameter	key)
        {
            return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(
                algorithm, passPhrase, salt, iterationCount, key).GetEncoded();
        }
    }
}
